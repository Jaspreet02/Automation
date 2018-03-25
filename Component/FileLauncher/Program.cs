using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLW.Lib.Log;
using BLW.Lib.SqliteHelper.Tables;

namespace BLW.Module.Standard.FileLauncher
{
    class Program
    {
        /// <summary>
        /// Entry point of application
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {
            var systemGuid = string.Empty;
            try
            {
                #region Validate Command Arguments

                if (args.Length == 0)
                    throw new ArgumentOutOfRangeException("No argument supplied for run Trigger manager");

                if (String.IsNullOrEmpty(args[0]))
                    throw new ArgumentOutOfRangeException("System Guid not supplied");
                else
                    systemGuid = Convert.ToString(args[0]);

                #endregion

                try
                {
                    LogInitializer.InitializeLogger("FileLauncher");
                    SingletonLogger.Instance.Info("Assembly Version Infomration : " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    SingletonLogger.Instance.Debug("App Instance Launcher Started.");
                    Processing objProcessing = new Processing();
                    objProcessing.StartProcessing();
                    SingletonLogger.Instance.Debug("App Instance Launcher Completed successfully.");
                }
                catch (Exception ex)
                {
                    SingletonLogger.Instance.Error("Error in instance launcher" + ex.ToString());
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
                            SingletonLogger.Instance.Error("Error while removing column. GUID = " + systemGuid);
                        }
                    }
                    catch (Exception ex2)
                    {
                        SingletonLogger.Instance.Debug("Error while removing column. GUID = " + ex2.ToString());
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                //If errors, then send mail and exit (just exit)          
                SingletonLogger.Instance.Fatal(ex.ToString());
            }
        }
    }
}
