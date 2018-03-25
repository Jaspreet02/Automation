using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLW.Lib.Log;
using BLW.Lib.CoreUtility;
using System.IO;
using BLW.Lib.SqliteHelper.Tables;

namespace BLW.Lib.CompHelper
{
    public class PeriodicComponent : IBLWComponent
    {  
        string systemGuid = string.Empty;
      
        #region IBLWComponent Members

        public bool Initialize(string[] args)
        {
#if DEBUG
            Console.WriteLine("Starting Debugging Mode..");
#else
            Console.WriteLine("Starting Production Mode..");
#endif

            //Initializing variables
            LogInitializer.InitializeLogger(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            //show current assembly version            
            SingletonLogger.Instance.Info("Assembly Version Infomration : " +
                                  System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

#if !DEBUG
            //Validate command line arguments
            #region Validate Arguments
            SingletonLogger.Instance.Debug("Validating command arguments..");            
            if(args.Length == 0)
            {
                SingletonLogger.Instance.Debug("System generated GUID not passed to.");
                return false;
            }            
            var systemGuid = args[0];
            SingletonLogger.Instance.Debug(" System generated GUID = " + systemGuid);            
            #endregion
#endif
            return true;
        }
        [Obsolete("Not using in Periodic Application Components",true)]
        public TriggerFileReader ProcessTriggerFile()
        {
            throw new InvalidOperationException("This functionality not supported for periodic components.");
        }

        public PatternMatchingMapper GetMapper()
        {
            throw new InvalidOperationException("This functionality not supported for periodic components.");
        }

        public bool Completed()
        {
#if !DEBUG
            SingletonLogger.Instance.Debug("Process compeleted successfully.");          
#endif
            return true;
        }

        public bool Error(Exception ex)
        {
#if !DEBUG
            SingletonLogger.Instance.Error("Error - (base exception) : " + ex.GetBaseException().ToString());
            SingletonLogger.Instance.Error("Error - " + ex.ToString());
            SingletonLogger.Instance.Error("Error in application. Error message : " + ex.Message + ". Error Detail : " + ex.StackTrace);
            SingletonLogger.Instance.Debug("Process errored out.");          
#endif
            return true;
        }

        public bool DeleteDbEntry()
        {
            #region Deleting Entry from Transaction Table
            // SQLITE Database Initialization
            Transaction trans = Transaction.Get(systemGuid);
            if(Transaction.Delete(systemGuid))
            {
                if(trans != null)
                    Licence.UpdateLastRunEndTime(trans.ExeName, DateTime.Now);
                Console.WriteLine("Successfully removed column. GUID = " + systemGuid);
            }
            else
            {
                Console.WriteLine("Error while removing column. GUID = " + systemGuid);
            }
            #endregion
            return true;
        }

        #endregion
    }
}
