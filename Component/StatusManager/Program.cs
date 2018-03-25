using System;
using System.Collections.Generic;
using System.Linq;
using BLW.Lib.Log;
using BLW.Lib.SqliteHelper.Tables;

namespace ApplicationStatusManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            var systemGuid = string.Empty;
            try
            {
                LogInitializer.InitializeLogger("Status Manager");

                SingletonLogger.Instance.Debug("Application Status Manager Component started.");

                #region Validate Command Arguments

                if (args.Length == 0)
                    throw new ArgumentOutOfRangeException("No argument supplied for run Status Manager");

                if (String.IsNullOrEmpty(args[0]))
                    throw new ArgumentOutOfRangeException("System Guid not supplied");
                else
                    systemGuid = Convert.ToString(args[0]);

                #endregion

                StatusUpdater objStatusUpdater = new StatusUpdater();

                // Get all status location to check status file.
                List<string> statusFileList = objStatusUpdater.GetAllActiveComponentTriggerAndStatusFileLocation().Select(x => x.StepStatusLocation).Distinct().ToList();
                SingletonLogger.Instance.Debug("Total Component status location " + statusFileList.Count());

                if (statusFileList.Count() > 0)
                {
                    SingletonLogger.Instance.Debug("Total " + statusFileList.Count() + " Status locations found for status file update.");
                    //Process for each location for status file update.
                    foreach (var item in statusFileList)
                    {
                        try
                        {
                            SingletonLogger.Instance.Debug("Status File location = " + item);
                            objStatusUpdater.UpdateStatus(item);
                        }
                        catch (Exception ex)
                        {
                            SingletonLogger.Instance.Info(ex.ToString());
                            continue;
                        }
                    }
                }
                else
                    SingletonLogger.Instance.Debug("No Status location found for status file update.");
            }
            catch (Exception ex)
            {
                SingletonLogger.Instance.Error(ex.ToString());
            }
            finally
            {
                #region Deleting Entry from Transaction Table
                try
                {

                    // SQLITE Database Initialization
                    Transaction trans = Transaction.Get(systemGuid);
                    if (Transaction.Delete(systemGuid))
                    {
                        if (trans != null)
                            Licence.UpdateLastRunEndTime(trans.ExeName, DateTime.Now);
                        SingletonLogger.Instance.Debug("Successfully removed column. GUID = " + systemGuid);
                    }
                    else
                    {
                        SingletonLogger.Instance.Debug("Error while removing column. GUID = " + systemGuid);
                    }
                }
                catch (Exception ex2)
                {
                    SingletonLogger.Instance.Debug("Error while removing column. GUID = " + ex2.ToString());
                }
                #endregion
            }
            SingletonLogger.Instance.Debug(" Application Status Manager Component ended.");
        }
    }
}
