using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLW.Lib.Log;
using BLW.Lib.CoreUtility;
using System.IO;
using BLW.Lib.SqliteHelper.Tables;
using System.Configuration;

namespace BLW.Lib.CompHelper
{
    public class TriggerBasedComponent : IBLWComponent
    {

        string invokerTriggerFile = string.Empty;
        string triggerFile = string.Empty;
        string systemGuid = string.Empty;
        string componentStartDate = DateTime.Now.ToString();
        TriggerFileReader triggerFileDetail;
        PatternMatchingMapper mapper;
        XmlHelper objXmlHelper;

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
            if(args.Length < 2)
            {
                SingletonLogger.Instance.Debug("Invalid arguments passed, Expected  'trigger file path' and and 'System generated GUID'.");
                return false;
            }
            invokerTriggerFile = args[0];
            systemGuid = args[1];

            SingletonLogger.Instance.Debug(" Trigger file = " + invokerTriggerFile);
            SingletonLogger.Instance.Debug(" System generated GUID = " + systemGuid);
            #endregion
#endif
            return true;
        }

        public TriggerFileReader ProcessTriggerFile()
        {
            #region Move trigger file to Component trigger location
            //Here we are moving file from invoke location to component trigger location
            //so that invoker doesn't pick it again & again
            if(Directory.Exists(ConfigurationManager.AppSettings["TriggerFileDirectory"]))
                Directory.CreateDirectory(ConfigurationManager.AppSettings["TriggerFileDirectory"]);

            if(invokerTriggerFile == null)
                throw new ArgumentNullException("Invoker trigger file is null or empty.");

            if(!File.Exists(invokerTriggerFile))
            {
                SingletonLogger.Instance.Debug("Trigger file not found at " + invokerTriggerFile + " ");
                return null;
            }

            //component trigger file location
            triggerFile = Path.Combine(ConfigurationManager.AppSettings["TriggerFileDirectory"], Path.GetFileName(invokerTriggerFile));
#if DEBUG
            File.Delete(triggerFile);
            File.Copy(invokerTriggerFile, triggerFile);
#else
            File.Move(invokerTriggerFile, triggerFile);
#endif
            SingletonLogger.Instance.Debug(string.Format("Trigger file successfully copied at  {0} location.", triggerFile));

            #endregion

            #region Read Trigger file
            objXmlHelper = new XmlHelper();
            SingletonLogger.Instance.Debug("Process start reading Trigger XML file " + triggerFile);
            var objTriggerFileReader = new TriggerFileReader { TriggerFileLocaton = triggerFile };
            triggerFileDetail = objTriggerFileReader.GetTriggerFileDetail();
            if(triggerFileDetail == null) throw new ArgumentNullException("No trigger File Detail");
            #endregion
            return triggerFileDetail;
        }

        public PatternMatchingMapper GetMapper()
        {
            if(mapper == null)
            {
                #region Set mapper
                mapper = new PatternMatchingMapper();
                mapper.SetClientAndAppDetails(triggerFileDetail.RunNumber);
                #endregion
            }
            return mapper;
        }

        public bool Completed()
        {
#if !DEBUG
            objXmlHelper.WriteComponentStatusInTriggerFile(triggerFile, componentStartDate, DateTime.Now.ToString());
            File.Move(triggerFile, triggerFileDetail.ComponentStatusDirectory + "\\status_" + Path.GetFileName(triggerFile));
#endif
            return true;
        }

        public bool Error(Exception ex)
        {
#if !DEBUG
            objXmlHelper.WriteComponentStatusInTriggerFile(triggerFile, componentStartDate, DateTime.Now.ToString(), "Error", ex.GetBaseException().ToString());
            File.Move(triggerFile, triggerFileDetail.ComponentStatusDirectory + "\\status_" + Path.GetFileName(triggerFile));
            SingletonLogger.Instance.Error("Error - (base exception) : " + ex.GetBaseException().ToString());
            SingletonLogger.Instance.Error("Error - " + ex.ToString());
            SingletonLogger.Instance.Error("Error in application. Error message : " + ex.Message + ". Error Detail : " + ex.StackTrace);
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
