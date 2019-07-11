using System;
using System.Net;
using System.Net.Mail;
using BLW.Lib.Log;
using BLW.Lib.SqliteHelper.Tables;

namespace EmailSenderApp
{
    class Program
    {
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

                LogInitializer.InitializeLogger("Email Sender");
                SingletonLogger.Instance.Debug("Email Sender Component starts.");
                SingletonLogger.Instance.Info("Assembly Version Infomration : " +
                                           System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

                Processing obj = new Processing();
                obj.SendEmails();
            }
            catch (Exception ex)
            {
                SingletonLogger.Instance.Error(ex.GetBaseException().ToString());
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
