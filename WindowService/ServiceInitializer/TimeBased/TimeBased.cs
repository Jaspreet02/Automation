using System;
using System.Linq;
using BLW.Lib.SqliteHelper;
using System.IO;
using BLW.Lib.Log;
using BLW.Lib.SqliteHelper.Tables;
namespace BLW.Modules.WindowsService.ServiceInitializer.TimeBased
{
    class TimeBased : IProduct
    {
        public void Processing()
        {
            SingletonLogger.Instance.Info("Window service start processing Time base executable files...");
            // Get all Time based licenses   
            var getAllLicense = Licence.GetAllTimebase();

            if(getAllLicense.Count() == 0)
            {
                SingletonLogger.Instance.Error("Executable file not configured in 'Licence' table.");
                return;
            }

            // Traverse, run if they needs to be run
            foreach(var license in getAllLicense)
            {
                //Check if time is come to run component               
                if(license.LastRunEnd.HasValue)
                {
                    var isRunningTime = DateTime.Now >= license.LastRunEnd.Value.AddMinutes(license.TimeInterval);
                    if(!isRunningTime)
                    {
                        //Running time is not came skip the EXE
                        SingletonLogger.Instance.Error("Running time is not came skip the EXE " + license.ExePath);
                        continue;
                    }
                }
                var noOfRunningInstances = license.RunningInstances;
                // Check number of instances are already running       
                if(noOfRunningInstances > 0)
                {
                    SingletonLogger.Instance.Error("Executable file '" + license.ExePath + "' already running, service found entry in 'Transaction' database.");
                    continue;
                }
                SingletonLogger.Instance.Debug("Total number of licenses in License table are " + license.ConcurrentLicences);
                SingletonLogger.Instance.Debug("Number of EXE are already running " + noOfRunningInstances);
                if(noOfRunningInstances >= license.ConcurrentLicences)
                {
                    SingletonLogger.Instance.Info("License exceeds for [" + Path.GetFileName(license.ExePath) + "], So not invoking the application for now.");
                    continue;
                }
                string systemGuid = System.Guid.NewGuid().ToString();
                SingletonLogger.Instance.Error("GUID = " + systemGuid);
                // Check executable file exist or not
                if(!File.Exists(license.ExePath))
                {
                    SingletonLogger.Instance.Error(Path.GetFileName(license.ExePath) + " not found at " + license.ExePath + " location.");
                    continue;
                }

                // Execute EXE and insert system generated GUID in database
                try
                {
                    InvokeExeAndDatabaseInsertion(license.ExePath, systemGuid);
                }
                catch(Exception ex)
                {
                    SingletonLogger.Instance.Error(ex.Message);
                    continue;
                }
            }
        }

        /// <summary>
        /// Invoke executable file, insert GUID and EXE information into Transaction table.
        /// </summary>
        /// <param name="exePath">Executable file path</param>
        /// <param name="systemGuid">System GUIID</param>
        /// <param name="triggerFilePath">Trigger file path</param>
        public void InvokeExeAndDatabaseInsertion(string exePath, string systemGuid)
        {
            try
            {
                #region Make an entry in table that EXE is running now
                Transaction newTransaction = new Transaction(systemGuid);
                newTransaction.ExeName = Path.GetFileName(exePath);
                newTransaction.Application = "NA";
                newTransaction.StartedAt = DateTime.Now;
                newTransaction.Enabled = true;
                var IsNewCreated = newTransaction.Save(true);
                #endregion

                #region Call EXE using trigger file
                SingletonLogger.Instance.Debug("Service start executing the " + exePath);
                TimeIntervalBasedInvoke timeIntervalInvoker = new TimeIntervalBasedInvoke(exePath, systemGuid);
                System.Diagnostics.Process proc = timeIntervalInvoker.Invoke();
                SingletonLogger.Instance.Debug("Service successfully executed the " + exePath);
                if(IsNewCreated)
                {
                    newTransaction.ProcessId = proc.Id;
                    newTransaction.Save();
                    Licence.UpdateLastRunStartTime(exePath, proc.StartTime);
                    SingletonLogger.Instance.Debug("Service successfully inserted information in Transaction table.");
                }


                #endregion
            }
            catch(Exception ex)
            {
                throw new Exception("Error while invoking " + exePath, ex);
            }
        }


    }
}
