using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BLW.Lib.FileTransfer;
using BLW.Lib.FileValidation;
using BLW.Lib.Log;
using BLW.Lib.CoreUtility;
using DbHander;

namespace JobInit
{
    public class JobInitHelper
    {
        PatternMatchingMapper mapper = null;
        String runNumber = String.Empty;
        List<string> emailList = null;
        int runId = 0;
        IApplicationRepository objApplicationRepository;
        IClientRepository objClientRepository;
        IFileTransferSettingsRepository objFileTransferSettingsRepository;
        IApplicationFilesRepository objApplicationFileRepository;
        IRunDetailsRepository objRunDetailsRepository;
        IProcSessionsRepository objProcSessionsRepository;

        public JobInitHelper()
        {
            objApplicationRepository = new ApplicationRepository();
            objClientRepository = new ClientRepository();
            objFileTransferSettingsRepository = new FileTransferSettingsRepository();
            objApplicationFileRepository = new ApplicationFileRepository();
            objRunDetailsRepository = new RunDetailsRepository();
            objProcSessionsRepository = new ProcSessionsRepository();
        }

        /// <summary>
        /// Job Init processing
        /// </summary>
        /// <param name="sessionKey">Service generated GUID</param>
        /// <param name="appId">Application ID</param>
        /// <returns></returns>
        public List<RunNumberAndOutput> Processing(string sessionKey, int appId)
        {
            List<RunNumberAndOutput> objRunNumberAndOutputList = new List<RunNumberAndOutput>();
            emailList = new List<string>();
            try
            {
                String desLoc = String.Empty;
                SingletonLogger.Instance.Debug("Get application info");
                //Get application by id
                var appInfo = objApplicationRepository.Find(appId);
                if (appInfo == null)
                    throw new NullReferenceException("Application details not found in database for Application Id : " + appId);
                //Get applications Client information
                var clientInfo = objClientRepository.Find(appInfo.ClientId);
                if (clientInfo == null)
                    throw new NullReferenceException("Client details not found in database for Client Id : " + appInfo.ClientId);

                SingletonLogger.Instance.Debug("Client Id = " + clientInfo.ClientId + "\t Application Id : " + appId);
                SingletonLogger.Instance.Debug("Client Name = " + clientInfo.Name + "\t Application name : " + appInfo.Name);

                SingletonLogger.Instance.Debug("Adding Mapper...");
                mapper = new PatternMatchingMapper();
                mapper.SetCurrentDateFormat();
                mapper.SetClientAndAppDetails(clientInfo.Name, appInfo.Name);
                SingletonLogger.Instance.Debug("Mapper Added.");

                SingletonLogger.Instance.Debug("Get file transfer settings for application for File by Setting Id " + appInfo.FileTransferSettingId);
                var fileTransferSetting = objFileTransferSettingsRepository.Find(appInfo.FileTransferSettingId);
                if (fileTransferSetting == null)
                    throw new Exception("No file transfer setting details found for File transfersettingId " + appInfo.FileTransferSettingId);

                SingletonLogger.Instance.Debug("Get queue type for file transfer setting id " + fileTransferSetting.QueueTypeId);

                // Converting queue type from string, if not able to convert throw exception          
                SingletonLogger.Instance.Debug("Checking for queue type.");
                QuequeType type = (QuequeType)Enum.ToObject(typeof(QuequeType), fileTransferSetting.QueueTypeId);
                SingletonLogger.Instance.Debug("Queue type is : " + type.ToString());

                var applicationFiles = objApplicationFileRepository.GetApplicationFileListByAppID(appInfo.ApplicationId);
                if (applicationFiles == null)
                    throw new NullReferenceException("No app files (Input configuration) found in db for appId : " + appInfo.ApplicationId + "appName : " + appInfo.Name);

                if (applicationFiles.Count() == 0)
                    throw new InvalidDataException("No app files (Input configuration) found in db for appId : " + appInfo.ApplicationId + "appName : " + appInfo.Name);

                SingletonLogger.Instance.Debug(string.Format("{0} files found for current application. ", applicationFiles.Count()));

                SingletonLogger.Instance.Debug("Processing files...");
                var validationType = ValidationType.Default;
                if (appInfo.IsBatch)
                    validationType = ValidationType.Batch;

                var adaptorSettings = SetLocationAdptorSettings(fileTransferSetting, appInfo, type);

                if (adaptorSettings == null)
                    throw new NullReferenceException("Null validation adapter setting found when getting valid adapter.");

                // Initializing transfer manager
                IFileTransferAdapter transferAdaptor = FileTransferAdapter.GetFileTransferAdapter(adaptorSettings);

                if (transferAdaptor == null)
                    throw new NullReferenceException("Null transfer adapter found.");

                FileTransferManager transferManager = new FileTransferManager(transferAdaptor);

                if (transferManager == null)
                    throw new NullReferenceException("Null transfer manager found.");

                SingletonLogger.Instance.Debug("validationType : " + validationType + "applicationFiles :" + applicationFiles.FirstOrDefault() + " transferManager :" + transferManager);

                IValidationPlugin validationAdaptor = GetValidationAdptor(validationType, applicationFiles.ToArray(), transferManager);

                if (validationAdaptor == null)
                    throw new NullReferenceException("Null validation adapter found when getting valid adapter.");

                // Validate 
                if (validationAdaptor.Validate())
                {
                    SingletonLogger.Instance.Debug("file is  Validated by " + validationAdaptor + " now Ready to download");
                    int count = 1;
                    // If There are some valid files then insert run number information to DB.
                    if (validationAdaptor.ValidFiles.Count > 0 && validationAdaptor.Ready)
                    {
                        foreach (var file in validationAdaptor.ValidFiles)
                        {
                            RunNumberAndOutput objRunNumberAndOutput = new RunNumberAndOutput();

                            #region Create Job
                            switch (validationType)
                            {
                                case ValidationType.Default:
                                    //Create run number and add entry in database
                                    runNumber = GetRunNumber(clientInfo.Code, appInfo.Code, appId);
                                    SingletonLogger.Instance.Debug("file is  Run in Single processing. RunNumber = " + runNumber);
                                    runId = InsertRunDetails(appInfo.ApplicationId, runNumber);
                                    SingletonLogger.Instance.Debug("Run details is saved in db successfully.");
                                    break;
                                case ValidationType.Batch:
                                    //Create run number and add entry in database
                                    if (count == 1)
                                    {
                                        runNumber = GetRunNumber(clientInfo.Code, appInfo.Code, appId);
                                        runId = InsertRunDetails(appInfo.ApplicationId, runNumber);
                                    }
                                    SingletonLogger.Instance.Debug("Insert runDetails in db for RunNumber " + runNumber + " Client " + clientInfo.Name + " and Application " + appInfo.Name);
                                    break;
                                case ValidationType.Custom:
                                    throw new NotSupportedException("Functionality for custom validation not supported.");
                                default:
                                    throw new InvalidOperationException("Invalid Validation type found.");
                            }
                            #endregion

                            #region Countinue Common Processing
                            mapper.SetClientAndAppDetails(runNumber);
                            if (String.IsNullOrEmpty(appInfo.HotFolder))
                                throw new Exception("Destination location is null or empty for " + appInfo.Name);

                            desLoc = mapper.EvaluateString(appInfo.HotFolder);
                            SingletonLogger.Instance.Debug("file is  downloading at " + desLoc);
                            transferAdaptor.Settings.DestinationLocation = desLoc;

                            // Add Run number and destination location                            
                            objRunNumberAndOutput.Output = desLoc;

                            #endregion

                            #region Download files
                            // download valid files
                            if (transferManager.DownloadFile(file))
                            {
                                SingletonLogger.Instance.Debug(file + " file is  downloaded successfully.");
                                //Inserting details to db                                                                                                          
                                if (InsertRawFileDetails(runId, file, desLoc))
                                {
                                    SingletonLogger.Instance.Debug("file detail is saved in db in raw files");
                                    if (appInfo.IsArchive)
                                    {
                                        var arcvInpuFile = desLoc + "\\" + Path.GetFileName(file);
                                        mapper.SetFileFormat(arcvInpuFile);
                                        var outFile = mapper.EvaluateString(appInfo.ArchivePath + "\\" + appInfo.ArchiveFileName);
                                        Archieve(arcvInpuFile, outFile);
                                    }

                                    if (appInfo.IsFileMove)
                                    {
                                        try
                                        {
                                            SingletonLogger.Instance.Debug("Process start to delete file from " + file);
                                            transferManager.DeleteFile(file);
                                            SingletonLogger.Instance.Debug(file + " file is deleted from source location.");
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Create Run number and output List
                            switch (validationType)
                            {
                                case ValidationType.Default:    // Run each file as a single file
                                    objRunNumberAndOutput.RunNumber = runNumber;
                                    objRunNumberAndOutputList.Add(objRunNumberAndOutput);
                                    break;
                                case ValidationType.Batch:
                                    if (count == 1)   // If file downloading type is 'Batch' then no need to add directory in array, run file as a batch
                                    {
                                        objRunNumberAndOutput.RunNumber = runNumber;
                                        objRunNumberAndOutputList.Add(objRunNumberAndOutput);
                                    }
                                    break;
                                case ValidationType.Custom:
                                    throw new NotSupportedException("Functionality for custom validation not supported.");
                                default:
                                    throw new InvalidOperationException("Invalid Validation type found.");
                            }
                            #endregion

                            #region Email

                            emailList.Add(Path.GetFileName(file));

                            #endregion

                            count++;
                        }
                    }
                    else
                    {
                        SingletonLogger.Instance.Debug("No valid files found or Files are not ready to download.");
                        objProcSessionsRepository.UpdateBySessionKey(sessionKey, Convert.ToByte(JobStatusType.Complete));
                    }
                    objProcSessionsRepository.UpdateBySessionKey(sessionKey, Convert.ToByte(JobStatusType.Complete));
                    SingletonLogger.Instance.Debug("Update values in ProcSession corresponding key" + sessionKey + " Value" + Convert.ToByte(JobStatusType.Complete));
                }

                SingletonLogger.Instance.Debug("No valid files found or Files are not ready to download.");
                objProcSessionsRepository.UpdateBySessionKey(sessionKey, Convert.ToByte(JobStatusType.Complete));

                //Prepare for email
                SendInputEmail inputEmail = new SendInputEmail();
                if (emailList != null)
                    if (emailList.Count > 0)
                        inputEmail.SendInputFileEmail(appInfo, runId, emailList);
                SingletonLogger.Instance.Debug("Email has been sent");
                return objRunNumberAndOutputList;
            }
            catch (Exception ex)
            {
                objProcSessionsRepository.UpdateBySessionKey(sessionKey, Convert.ToByte(JobStatusType.Error));
                throw new Exception("Error in Job-init " + ex);
            }
        }


        /// <summary>
        /// Setting for file downloading
        /// </summary>
        /// <param name="fileTransferSetting">File Transfer Setting</param>
        /// <param name="app">Application detail</param>
        /// <param name="type">Queue type SFTP/Local</param>
        /// <returns></returns>
        public IAdaperSettings SetLocationAdptorSettings(FileTransferSetting fileTransferSetting, Application app, QuequeType type)
        {
            try
            {
                if (string.IsNullOrEmpty(app.InputPath))
                    throw new InvalidDataException("Input Location cannot be null or empty.");

                IAdaperSettings adaptorSettings = new FileTransferSettingOpration();
                adaptorSettings.Host = string.IsNullOrEmpty(fileTransferSetting.Host) ? "" : fileTransferSetting.Host.Trim();
                adaptorSettings.UserName = string.IsNullOrEmpty(fileTransferSetting.UserName) ? "" : fileTransferSetting.UserName.Trim();
                if (!string.IsNullOrEmpty(fileTransferSetting.Password))
                {
                    adaptorSettings.Password = fileTransferSetting.Password; //clientService.DecodeFrom64(fileTransferSetting.Password);
                }
                adaptorSettings.SourceLocation = app.InputPath;
                int port = 0;
                int.TryParse(fileTransferSetting.Port, out port);

                //Processing default
                switch (type)
                {
                    case QuequeType.SFTP:
                        if (port <= 0)
                            port = 22;
                        adaptorSettings.Type = "SFTP";
                        break;
                    case QuequeType.FTP:
                        if (port <= 0)
                            port = 21;
                        adaptorSettings.Type = "FTP";
                        break;
                    case QuequeType.SharedPath:
                        adaptorSettings.Port = 0;
                        adaptorSettings.Type = "SharedPath";
                        break;
                    case QuequeType.LocalFileSystem:
                        adaptorSettings.Port = 0;
                        adaptorSettings.Type = "LocalFileSystem";
                        break;
                    default:
                        throw new InvalidOperationException("Invalid Queue type {0}.\nAvailable Queue types are : \n1. FTP  \n2. SFTP  \n2. SharedPath  \n4. LocalFileSystem");
                }
                adaptorSettings.Port = port;
                return adaptorSettings;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validate File 
        /// </summary>
        /// <param name="validationType">Is batch / Default / custom</param>
        /// <param name="applicationFiles">Application files</param>
        /// <param name="transferManager">File transfer manager</param>
        /// <returns></returns>
        public IValidationPlugin GetValidationAdptor(ValidationType validationType, ApplicationFile[] applicationFiles, FileTransferManager transferManager)
        {
            IValidationPlugin validationAdaptor = null;
            try
            {
                switch (validationType)
                {
                    case ValidationType.Default:
                        validationAdaptor = new DefaultValidation(transferManager, applicationFiles.Select(T => T.Mask).ToList());
                        break;
                    case ValidationType.Batch:
                        var validateFileInfos = (from appFile in applicationFiles
                                                 select new ValidationFileInfo(appFile.IsRequired, appFile.Mask)).ToList();
                        ///
                        validationAdaptor = new BatchValidation(transferManager, validateFileInfos);
                        break;
                    case ValidationType.Custom:
                        throw new NotSupportedException("Functionality for custom validation not supported.");
                    default:
                        throw new InvalidOperationException("Invalid Validation type found.");

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return validationAdaptor;
        }

        /// <summary>
        /// Insert detail into Raw File detail table
        /// </summary>
        /// <param name="runId">Run number Id</param>
        /// <param name="file">Raw File Name</param>
        /// <param name="desLoc"></param>
        /// <returns></returns>
        public bool InsertRawFileDetails(int runId, string file, string desLoc)
        {
            try
            {
                using (IRawFileRepository objRawFileRepository = new RawFileRepository())
                {
                    RawFile rawFile = new RawFile();
                    rawFile.RunNumberId = runId;
                    rawFile.FileName = Path.GetFileName(file);
                    rawFile.HotFolder = desLoc;
                    rawFile.Status = true;
                    objRawFileRepository.Save(rawFile);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting record in Raw file : " + ex);
            }
            return true;
        }

        /// <summary>
        /// Insert Run number detail Into RunDetails table
        /// </summary>
        /// <param name="appId">Application Id</param>
        /// <param name="runNumber"></param>
        /// <returns></returns>
        public int InsertRunDetails(int appId, string runNumber)
        {
            try
            {
                var runNumberInfo = new RunDetail();
                runNumberInfo.ApplicationId = appId;
                runNumberInfo.RunNumber = runNumber;
                runNumberInfo.RunNumberStatusId = 0;
                runNumberInfo.Status = true;
                return objRunDetailsRepository.Save(runNumberInfo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in insert details in Run Details in job init component : " + ex);
            }
        }


        public bool Archieve(string inputFile, string outputFile)
        {
            try
            {
                if (!File.Exists(inputFile))
                    throw new FileNotFoundException("No file found for archive in job init : " + inputFile);

                if (String.IsNullOrEmpty(outputFile))
                    throw new Exception("Output file for archive can not null or empty in job init.");

                var archivePath = Path.GetDirectoryName(outputFile);

                if (!Directory.Exists(archivePath))
                    Directory.CreateDirectory(archivePath);

                File.Copy(inputFile, outputFile, false);
                SingletonLogger.Instance.Debug("File Archieved at " + archivePath);
            }
            catch (Exception ex)
            {
                throw new Exception("Error when archive the file : " + ex);
            }
            return true;
        }


        /// <summary>
        /// Get latest run number
        /// </summary>
        /// <param name="clientCode">Client short name</param>
        /// <param name="appCode">Application short name</param>
        /// <returns></returns>
        public string GetRunNumber(string clientCode, string appCode, int appId)
        {
            try
            {

                var lastRunNumber = objRunDetailsRepository.GetLastRunNumber(appId);
                if (string.IsNullOrEmpty(lastRunNumber))
                    lastRunNumber = "000000";
                RunNumber objRunNumber = new RunNumber();
                var runNumber = objRunNumber.GetRunNumber(clientCode, appCode, lastRunNumber);
                if (String.IsNullOrEmpty(runNumber))
                    throw new Exception("RunNumber cannot be empty or null");
                else
                    return runNumber;
            }
            catch (Exception ex)
            {
                throw new Exception("Error when get runNo" + ex);
            }
        }
    }


    public class RunNumberAndOutput
    {
        public string RunNumber { get; set; }
        public string Output { get; set; }
    }
}
