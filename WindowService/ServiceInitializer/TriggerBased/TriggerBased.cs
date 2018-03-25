using System;
using System.Linq;
using System.IO;
using BLW.Lib.SqliteHelper;
using BLW.Lib.Log;
using System.Configuration;
using BLW.Lib.SqliteHelper.Tables;
using BLW.Lib.CoreUtility;
using BLW.Lib.CoreUtility.Exceptions;

namespace BLW.Modules.WindowsService.ServiceInitializer.TriggerBased
{
    class TriggerBased : IProduct
    {
        public void Processing()
        {
            SingletonLogger.Instance.Debug("Service start initializing Trigger based process...");

            var triggerFiles = Directory.GetFiles(ConfigurationManager.AppSettings["TriggerFileDir"], "*.xml");
            if (!triggerFiles.Any())
            {
                SingletonLogger.Instance.Debug("Trigger files are not found at " + ConfigurationManager.AppSettings["TriggerFileDir"] + " location.");
                return;
            }

            SingletonLogger.Instance.Debug(triggerFiles.Count() + " trigger file ready to run.");
            // Read all trigger files and Invoke executable file
            foreach (var triggerFile in triggerFiles)
            {
                SingletonLogger.Instance.Debug("Trigger file location " + triggerFile);
                string systemGuid = System.Guid.NewGuid().ToString();

                // Read Component excitable file location from Trigger file
                var objTrigger = new TriggerFileOpration { TriggerFilePath = triggerFile };
                string exeFile = string.Empty;
                try
                {
                    exeFile = objTrigger.ReadExcutableFileLocationFromTriigerFile();
                }
                catch (IOBLWException iEx) //If file is already in use then don't process 
                {
                    SingletonLogger.Instance.Warning("File is already in use. " + iEx.ToString());
                    continue;
                }
                SingletonLogger.Instance.Debug("EXE file location " + exeFile);
                if (string.IsNullOrEmpty(exeFile))
                {
                    SingletonLogger.Instance.Error("Executable file invalid : " + exeFile);
                    continue;
                }
                if (!File.Exists(exeFile))
                {
                    SingletonLogger.Instance.Error("Executable file not found at " + exeFile + " location,");
                    continue;
                }

                // Check EXE those are in trigger file is also exist into License file
                var license = Lib.SqliteHelper.Tables.Licence.Get(exeFile);

                if (license == null)
                {
                    SingletonLogger.Instance.Error("Licence not configured for '" + exeFile + "' file in database.");
                    continue;
                }
                if (!license.Enabled)
                {
                    SingletonLogger.Instance.Error("Licence is disabled for '" + exeFile + "' file in database, So not launching this exe");
                    continue;
                }

                if (license.ConcurrentLicences == Constants.UNLIMITED_INSTANCES)
                {
                    SingletonLogger.Instance.Debug("Total number of licenses are unlimited");
                    // Call EXE and database insertion
                    InvokeExeAndDatabaseInsertion(exeFile, systemGuid, triggerFile);
                    SingletonLogger.Instance.Info("Service successfully inserted information in database.");
                }
                else
                {
                    var noOfRunningInstances = license.RunningInstances;
                    SingletonLogger.Instance.Debug("Total number of licenses in License table are " + license.ConcurrentLicences);
                    SingletonLogger.Instance.Debug("Number of EXE are already running " + noOfRunningInstances);
                    if (noOfRunningInstances >= license.ConcurrentLicences)
                    {
                        SingletonLogger.Instance.Info("License exceeds for [" + Path.GetFileName(license.ExePath) + "], So not invoking the application for now.");
                        continue;
                    }
                    // Call EXE and database insertion
                    InvokeExeAndDatabaseInsertion(exeFile, systemGuid, triggerFile);
                    SingletonLogger.Instance.Debug("Service successfully inserted information in database.");
                }
            }
            SingletonLogger.Instance.Debug("Service successfully completed Trigger based process...");
        }

        /// <summary>
        /// Invoke executable file, insert GUID and EXE information into Transaction table.
        /// </summary>
        /// <param name="exePath">Executable file path</param>
        /// <param name="systemGuid">System GUIID</param>
        /// <param name="triggerFilePath">Trigger file path</param>
        public void InvokeExeAndDatabaseInsertion(string exePath, string systemGuid, string triggerFilePath)
        {
            TriggerFileReader objTriggerFileReader = new TriggerFileReader();
            objTriggerFileReader.TriggerFileLocaton = triggerFilePath;
            var triggerFileDetail = objTriggerFileReader.GetTriggerFileDetail();
            #region Make an entry in table that EXE is running now
            Transaction newTransaction = new Transaction(systemGuid);
            newTransaction.ExeName = Path.GetFileName(exePath);
            newTransaction.Application = triggerFileDetail.ApplicationName != null ? triggerFileDetail.ApplicationName : "NA";
            newTransaction.StartedAt = DateTime.Now;
            newTransaction.Enabled = true;
            var IsNewCreated = newTransaction.Save(true);
            #endregion

            #region Call EXE using trigger file
            SingletonLogger.Instance.Debug("Service start executing the " + exePath);
            TriggerBasedInvoke triggerInvoker = new TriggerBasedInvoke(exePath, triggerFilePath, systemGuid);
            System.Diagnostics.Process proc = triggerInvoker.Invoke();
            SingletonLogger.Instance.Debug("Service successfully executed the " + exePath);
            if (IsNewCreated)
            {
                newTransaction.ProcessId = proc.Id;
                newTransaction.Save();
                Licence.UpdateLastRunStartTime(exePath, proc.StartTime);
                SingletonLogger.Instance.Debug("Service successfully inserted information in Transaction table.");
            }
            #endregion
        }

    }
}
