using BLW.Lib.CoreUtility;
using BLW.Lib.Log;
using BLW.Lib.TransferManager.Setting;
using DbHander;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Uploader
{

    public class Processing
    {
        #region variable

        static string componentStartDate = DateTime.Now.ToString();
        readonly string TriggerPath;
        PatternMatchingMapper mapper = null;
        XmlHelper objXmlHelper = null;
        string StatusDirectory = string.Empty;
        TransferManager Manager = null;
        IRunDetailsRepository _runDetailRepository;
        IRunComponentStatusRepository _runComponentStatusRepository;
        IUploadFileRepository _uploadFileRepository;
        IFileTransferSettingsRepository _fileTransferSettingRepository;
        IApplicationRepository test;

        #endregion

        #region Constructor

        public Processing(string trigger)
        {
            try
            {
                TriggerPath = trigger;
                objXmlHelper = new XmlHelper();
                _runDetailRepository = new RunDetailsRepository();
                _runComponentStatusRepository = new RunComponentStatusRepository();
                _uploadFileRepository = new UploadFileRepository();
                _fileTransferSettingRepository = new FileTransferSettingsRepository();
                test = new ApplicationRepository();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Methods

        public void Run()
        {
            try
            {

                #region Trigger file details

                SingletonLogger.Instance.Debug("Process start reading Trigger XML file from " + TriggerPath);
                TriggerFileReader objTriggerFileReader = new TriggerFileReader();
                objTriggerFileReader.TriggerFileLocaton = TriggerPath;
                var triggerFileDetail = objTriggerFileReader.GetTriggerFileDetail();
                StatusDirectory = triggerFileDetail.ComponentStatusDirectory + "\\status_" + Path.GetFileName(TriggerPath);
                SingletonLogger.Instance.Debug("Process successfully read trigger XML file.");

                #endregion

                #region Mapper Setting

                mapper = new PatternMatchingMapper();
                mapper.SetCurrentDateFormat();
                mapper.SetClientAndAppDetails(triggerFileDetail.RunNumber);
                mapper.SetClientAndAppDetails(triggerFileDetail.ClientName, triggerFileDetail.ApplicationName);

                #endregion

                #region Run Process
                var application = test.Find(1);
                var runItem = _runComponentStatusRepository.Find(triggerFileDetail.RunComponentStatusId).RunNumberId;
                var allCompeletedComponents = _runComponentStatusRepository.FindAll().Where(x => x.RunNumberId == runItem && x.ComponentStatusId == (byte)ComponentStatusType.Completed).Select(x => x.ComponentId).ToList();
                var applicationId = _runDetailRepository.GetApplicationIdByRunNumber(triggerFileDetail.RunNumber);
                SingletonLogger.Instance.Debug(allCompeletedComponents.Count() + " component has been completed.");
                foreach (var item in allCompeletedComponents)
                {
                    var recordList = _uploadFileRepository.FindAllByAppNComponentId(applicationId, item);
                    foreach (var recordItem in recordList)
                    {
                        List<string> fileList = new List<string>();
                        var inputPath = mapper.EvaluateString(recordItem.FileInputPath);
                        foreach (var mask in recordItem.InputFileMask.Split('|').ToList())
                        {
                            Regex reg = new Regex(mask.Trim());
                            fileList.AddRange(Directory.GetFiles(inputPath).Where(path => reg.IsMatch(path)).ToList());
                        }
                        if (fileList.Count > 0)
                        {
                            SingletonLogger.Instance.Debug(fileList.Count + " files found for move from " + inputPath);
                            if (recordItem.IsArchiveOutputRequired)
                            {
                                var archiveName = mapper.EvaluateString(recordItem.ArchiveFileExpression);
                                var fileTransferSetting = _fileTransferSettingRepository.Find(recordItem.ArchiveFileTransferSettingId);
                                var outputPath = mapper.EvaluateString(recordItem.ArchiveOutputPath);
                                Manager = new TransferManager(SetAdaptorSetting(fileTransferSetting, outputPath));
                                Manager.Processing(fileList);
                                SingletonLogger.Instance.Debug(fileList.Count + " files has been archived successfully.");
                            }
                            if (recordItem.IsMoveFileRequired)
                            {
                                var moveName = mapper.EvaluateString(recordItem.MoveFileExpression);
                                var fileTransferSetting = _fileTransferSettingRepository.Find(recordItem.MoveFileTransferSettingId);
                                var outputPath = mapper.EvaluateString(recordItem.MoveFilePath);
                                Manager = new TransferManager(SetAdaptorSetting(fileTransferSetting, outputPath));
                                Manager.Processing(fileList);
                                SingletonLogger.Instance.Debug(fileList.Count + " files has been moved successfully.");
                            }
                        }
                    }
                }

                SingletonLogger.Instance.Debug("Files has been uploaded successfully.");

                #endregion

                #region Move trigger file with status

                objXmlHelper.WriteComponentStatusInTriggerFile(TriggerPath, componentStartDate, DateTime.Now.ToString());
                File.Move(TriggerPath, StatusDirectory);

                #endregion
            }
            catch (Exception ex)
            {
                TriggerStatus(ex);
            }
        }


        public IAdaperSetting SetAdaptorSetting(FileTransferSetting fileTransferSetting, string outputPath)
        {
            IAdaperSetting adaptorSettings = new FileTransferSettings();
            adaptorSettings.Host = !string.IsNullOrEmpty(fileTransferSetting.Host) ? fileTransferSetting.Host.Trim() : "";
            adaptorSettings.UserName = !string.IsNullOrEmpty(fileTransferSetting.UserName) ? fileTransferSetting.UserName.Trim() : "";

            if (fileTransferSetting.Password != null && !string.IsNullOrEmpty(fileTransferSetting.Password.Trim()))
            {
                //TODO
                // adaptorSettings.Password = clientService.DecodeFrom64(fileTransferSetting.Password);l n ,m;
            }

            adaptorSettings.DestinationLocation = outputPath;
            int port = 0;
            int.TryParse(fileTransferSetting.Port, out port);
            DbHander.QueueTypes type = (DbHander.QueueTypes)Enum.ToObject(typeof(DbHander.QueueTypes), fileTransferSetting.QueueTypeId);
            //Processing default
            switch (type)
            {
                case DbHander.QueueTypes.SFTP:
                    if (port <= 0)
                        port = 22;
                    adaptorSettings.Type = "SFTP";
                    break;
                case DbHander.QueueTypes.FTP:
                    if (port <= 0)
                        port = 21;
                    adaptorSettings.Type = "FTP";
                    break;
                case DbHander.QueueTypes.SharedPath:
                    adaptorSettings.Port = 0;
                    adaptorSettings.Type = "SharedPath";
                    break;
                case DbHander.QueueTypes.LocalFileSystem:
                    adaptorSettings.Port = 0;
                    adaptorSettings.Type = "LocalFileSystem";
                    break;
                default:
                    throw new InvalidOperationException("Invalid Queue type {0}.\nAvailable Queue types are : \n1. FTP  \n2. SFTP  \n2. SharedPath  \n4. LocalFileSystem");
            }
            adaptorSettings.Port = port;
            return adaptorSettings;
        }

        /// <summary>
        /// Move trigger file with error status in status directory.
        /// </summary>
        /// <param name="error"></param>
        private void TriggerStatus(Exception error)
        {
            objXmlHelper.WriteComponentStatusInTriggerFile(TriggerPath, componentStartDate, DateTime.Now.ToString(), "Error", error.Message);
            File.Move(TriggerPath, StatusDirectory);
            SingletonLogger.Instance.Error("Error in Archive Manager Process. Error message : " + error.Message + ". Error Detail : " + error.StackTrace);
        }

        #endregion
    }
}
