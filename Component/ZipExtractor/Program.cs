using System;
using System.Linq;
using Ionic.Zip;
using BLW.Lib.Log;
using System.IO;
using BLW.Lib.CoreUtility;
using System.Configuration;
using BLW.Lib.SqliteHelper.Tables;

namespace BLW.Modules.Helpers.ZipExtractor
{

    public class Program
    {
        public static string TriggerFileLocation { get; set; }
        public static PatternMatchingMapper mapper = null;
        static void Main(string[] args)
        {
            LogInitializer.InitializeLogger("ZipExtractor");
            SingletonLogger.Instance.Debug("Extractor component has been started.");
            var systemGuid = string.Empty;
            var triggerFile = string.Empty;

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
            TriggerFileLocation = AppConfig.GetValueByKey("TriggerFileDirectory");
            if (!Directory.Exists(TriggerFileLocation))
                Directory.CreateDirectory(TriggerFileLocation);
            TriggerFileLocation = Path.Combine(TriggerFileLocation, Path.GetFileName(triggerFile));
            //Move to intermediate directory                  
            File.Move(triggerFile, TriggerFileLocation);
            SingletonLogger.Instance.Debug("Trigger has been moved at " + TriggerFileLocation);

            #endregion

            XmlHelper objXmlHelper = new XmlHelper();
                var componentStartDate = DateTime.Now.ToString();

                SingletonLogger.Instance.Debug("Process start reading Trigger XML file " + TriggerFileLocation);
                TriggerFileReader objTriggerFileReader = new TriggerFileReader();
                objTriggerFileReader.TriggerFileLocaton = TriggerFileLocation;
                var triggerFileDetail = objTriggerFileReader.GetTriggerFileDetail();
                SingletonLogger.Instance.Debug("Process successfully read trigger XML file.");
                try
                {
                    mapper = new PatternMatchingMapper();
                    mapper.SetCurrentDateFormat();
                    mapper.SetClientAndAppDetails(triggerFileDetail.RunNumber);


                    var inputLocation = triggerFileDetail.InputDetails.FirstOrDefault();
                    var outputLocation = triggerFileDetail.OutputDetails.FirstOrDefault();
                    if (inputLocation == null)
                        throw new Exception("");

                    string inputLoc = mapper.EvaluateString(inputLocation.DirectoryLocation);
                    string outputLoc = mapper.EvaluateString(outputLocation.DirectoryLocation);
                    if (String.IsNullOrEmpty(inputLoc)) throw new Exception("Input loction is never be null or empty");
                    if (!Directory.Exists(inputLoc)) throw new Exception("Input loction not exists" + inputLoc);
                    var availableFiles = Directory.GetFiles(inputLoc, inputLocation.FileMask);
                  
                    if (String.IsNullOrEmpty(outputLoc)) throw new Exception("Output loction is never be null or empty");
                    if (!Directory.Exists(outputLoc)) Directory.CreateDirectory(outputLoc);


                    foreach (var zipfile in availableFiles)
                    {

                        // Start Extracting
                        SingletonLogger.Instance.Debug("Start processing " + Path.GetFileName(TriggerFileLocation) + " file.");
                        ExtractOperations.ExtractFile(zipfile, outputLoc);


                        if (AppConfig.FileOperation)
                            File.Delete(zipfile);

                    }

                    objXmlHelper.WriteComponentStatusInTriggerFile(TriggerFileLocation, componentStartDate, DateTime.Now.ToString());
                    File.Move(TriggerFileLocation, triggerFileDetail.ComponentStatusDirectory + "\\status_" + Path.GetFileName(TriggerFileLocation));
                }
                catch (Exception ex)
                {
                    objXmlHelper.WriteComponentStatusInTriggerFile(TriggerFileLocation, componentStartDate, DateTime.Now.ToString(), "Error", ex.GetBaseException().ToString());
                    File.Move(TriggerFileLocation, triggerFileDetail.ComponentStatusDirectory + "\\status_" + Path.GetFileName(TriggerFileLocation));
                    SingletonLogger.Instance.Error("Error in ZipExtractor. Error message : " + ex.Message + ". Error Detail : " + ex.StackTrace);
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
    public class ExtractOperations
    {
        public static void ExtractFile(string zipFile, string extractPath)
        {
            //Unzip files
            using (ZipFile zippedFile = ZipFile.Read(zipFile))
            {
                zippedFile.ExtractProgress += ExtractProgress;
                //Create directory same as zip file name               
                SingletonLogger.Instance.Debug(string.Format("Extracting zip file : {0}", zipFile));
                foreach (ZipEntry zippedEntry in zippedFile)
                {
                    try
                    {
                        string fileExtension = Path.GetExtension(zippedEntry.FileName);

                        //Create directory if not exists                        
                        if (!Directory.Exists(extractPath))
                            Directory.CreateDirectory(extractPath);
                        zippedEntry.Extract(extractPath, ExtractExistingFileAction.Throw);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error in extracting file", ex);
                    }
                }
            }
           


        }
        /// <summary>
        /// show progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            if (e.EventType == ZipProgressEventType.Extracting_EntryBytesWritten)
            {
                if (justHadByteUpdate)
                    Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write("Extracting  : {0}/{1} ({2:N0}%)", (e.BytesTransferred / 1024 / 1024).ToString("###,###,###") + "MB",
                    (e.TotalBytesToTransfer / 1024 / 1024).ToString("###,###,###") + "MB", e.BytesTransferred / (0.01 * e.TotalBytesToTransfer));
                justHadByteUpdate = true;
            }
        }
        static bool justHadByteUpdate = false;
    }
}
