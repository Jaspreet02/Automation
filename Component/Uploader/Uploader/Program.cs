using BLW.Lib.Log;
using BLW.Lib.SqliteHelper.Tables;
using System;
using System.IO;

namespace Uploader
{
    public class Program
    {
        static void Main(string[] args)
        {
            #region Variable Declarations

            var systemGuid = string.Empty;
            var triggerFile = string.Empty;

            #endregion

            try
            {
                LogInitializer.InitializeLogger("ArchiveManager");
                SingletonLogger.Instance.Debug("Archive Manager component has been started.");

                #region Validate Command Arguments

                // Start Processing 
                if (args.Length != 2)
                {
                    SingletonLogger.Instance.Debug("Number of passed arguments are Invalid.");
                    return;
                }
                else
                {
                    triggerFile = args[0];
                    systemGuid = args[1];
                }

                if (!File.Exists(triggerFile))
                {
                    SingletonLogger.Instance.Error("Trigger file does not exist at = " + triggerFile);
                    return;
                }
                else
                    SingletonLogger.Instance.Debug("Trigger file = " + triggerFile);


                if (String.IsNullOrEmpty(systemGuid))
                {
                    SingletonLogger.Instance.Error("Trigger file does not exist at = " + triggerFile);
                    return;
                }
                else
                    SingletonLogger.Instance.Debug("System generated GUID = " + systemGuid);

                #endregion

                #region Move trigger file from Invoker trigger location to Component trigger location

                // We are moving trigger file from invoker location to avoid reprocessing of same file
                var intermediateTriggerDirectory = AppConfig.GetValueByKey("TriggerFileDirectory");
                if (!Directory.Exists(intermediateTriggerDirectory))
                    Directory.CreateDirectory(intermediateTriggerDirectory);
                var intermediateTriggerFile = Path.Combine(intermediateTriggerDirectory, Path.GetFileName(triggerFile));
                //Move to intermediate directory                  
                File.Move(triggerFile, intermediateTriggerFile);
                SingletonLogger.Instance.Debug("Trigger has been moved at " + intermediateTriggerFile);

                #endregion

                #region Start processing

                Processing objProcessing = new Processing(intermediateTriggerFile);
                objProcessing.Run();

                SingletonLogger.Instance.Debug("Process of Archive Manager has been completed sucesfully.");
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
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
        }
    }
}
