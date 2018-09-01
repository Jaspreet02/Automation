using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using BLW.Lib.Log;
using System.Text.RegularExpressions;
using BLW.Lib.TransferManager.Setting;
using BLW.Lib.TransferManager;
using DbHander;

namespace BLW.Module.Standard.FileLauncher
{
    public class Processing
    {
        #region Variables
        
        List<String> _inputFileList = null;
        IFileTransferAdapter _transferAdapter = null;
        IAdaperSetting _setting = null;
        IApplicationRepository _applicationRepository;
        IProcComponantRepository _procComponentRepository;
        IFileTransferSettingsRepository _fileTransferRepository;
        IScheduledFrequencyRepository _scheduledRepository;
        IProcSessionsRepository _procSessionRepository;
        IApplicationFilesRepository _applicationFileRepository;

        #endregion

        #region Constructor
        public Processing()
        {
            _applicationRepository = new ApplicationRepository();
            _procComponentRepository = new ProcComponentRepository();
            _fileTransferRepository = new FileTransferSettingsRepository();
            _scheduledRepository = new ScheduledFrequencyRepository();
            _procSessionRepository = new ProcSessionsRepository();
            _applicationFileRepository = new ApplicationFileRepository();
        }

        #endregion

        public bool StartProcessing()
        {
            try
            {
                var applicationList = _applicationRepository.FindAllActive();
                SingletonLogger.Instance.Debug(applicationList.Count() + " application found in DB");
                var procComponent = _procComponentRepository.GetFirstRecord();
                var list = applicationList.GroupBy(x => new { x.FileTransferSettingId, x.InputPath });
                SingletonLogger.Instance.Debug(list.Count() + " distinct input path of application");
                _inputFileList = new List<string>();
                foreach (var item in list)
                {                  
                    try
                    {
                        var fileTransferSetting =_fileTransferRepository.Find(item.Key.FileTransferSettingId);
                        _setting = SetAdaptorSetting(fileTransferSetting);
                        _transferAdapter = FileTransferAdapter.GetFileTransferAdapter(_setting);
                        var fileList = _transferAdapter.ShowFileList(item.Key.InputPath);
                        SingletonLogger.Instance.Debug(fileList.Count + " files found at " + item.Key.InputPath);
                        foreach (var subitem in fileList)
                        {
                            Console.WriteLine("Files : " + subitem);
                            try
                            {
                                SingletonLogger.Instance.Debug("Checking file accessibility of " + subitem);
                                if (_transferAdapter.IsAccessible(subitem))
                                {
                                    _inputFileList.Add(Path.GetFileName(subitem));
                                }
                            }
                            catch (Exception ex)
                            {
                                SingletonLogger.Instance.Debug(ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SingletonLogger.Instance.Debug(ex.Message);
                    }
                }

                SingletonLogger.Instance.Debug(_inputFileList.Count + " file found ready to download");
                if (_inputFileList.Count() == 0)
                    return true;

                var appIdList = AppIdsbyMasks(_inputFileList.ToList());
                SingletonLogger.Instance.Debug(appIdList.Count() + " distinct application found for " + _inputFileList.Count + " file");
                foreach (var item in appIdList)
                {
                    var frequencyList = _scheduledRepository.GetScheduledFrequencyListbyAppId(item);
                    var currentTime = DateTime.Now.TimeOfDay;
                    bool IsValidTime = true;
                    foreach (var subItem in frequencyList)
                    {
                        if (subItem.StartTime < currentTime && subItem.EndTime > currentTime)
                        {
                            IsValidTime = true;
                            break;
                        }
                        else
                        {
                            IsValidTime = false;
                        }
                    }
                    if (IsValidTime)
                    {
                        SingletonLogger.Instance.Debug("ApplicationId = " + item + " has been started for processing");
                        if (InstanceLimitAvailable(item, procComponent.ProcComponentId, procComponent.LicenceLimit))
                        {
                            SingletonLogger.Instance.Debug("License available for applicationId = " + item);
                            int processId;
                            String sessionKey = LaunchInstance(procComponent.ExecutablePath, item, out processId);
                            SingletonLogger.Instance.Debug("Process has been launched for " + item);
                            var procSession = new ProcSession
                            {
                                CreatedAt = DateTimeOffset.Now,
                                ExpectedDateTime = DateTime.Now.AddMinutes(30),
                                KillRequired = false,
                                ProcComponentID = procComponent.ProcComponentId,
                                ProcessID = Convert.ToString(processId),
                                ApplicationId = item,
                                SessionKey = sessionKey,
                                ProcStatus = 1,
                                Status = true
                            };
                            if (_procSessionRepository.Save(procSession) > 0)
                                SingletonLogger.Instance.Debug("New record of proc session updated in database");
                        }
                        else
                        {
                            SingletonLogger.Instance.Debug("License doesn't available for applicationId = " + item);
                        }
                    }
                    else
                    {
                        SingletonLogger.Instance.Debug("Frequency time doesn't match for application.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        private String LaunchInstance(string exePath, int appId, out int processId)
        {
            try
            {
                SingletonLogger.Instance.Debug("Start to launch the exe");
                var proces = new System.Diagnostics.Process();
                proces.StartInfo.FileName = exePath;
                var currentProcessSessionKey = System.Guid.NewGuid();
                proces.StartInfo.Arguments = currentProcessSessionKey + " " + String.Join(" ", appId);
                SingletonLogger.Instance.Debug("Arguments" + proces.StartInfo.Arguments);
                proces.Start();
                processId = proces.Id;
                SingletonLogger.Instance.Debug("launched exe for session key" + currentProcessSessionKey);
                return Convert.ToString(currentProcessSessionKey);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<int> AppIdsbyMasks(List<String> fileList)
        {
            try
            {
                List<int> appIds = new List<int>();
                var maskList = _applicationFileRepository.FindAllActive();
                foreach (var item in fileList)
                {
                    Regex regex = null;
                    foreach (var subItem in maskList)
                    {
                        try
                        {
                            regex = new Regex(subItem.Mask);
                            Match match = regex.Match(item);
                            if (match.Success)
                            {
                                appIds.Add(subItem.ApplicationId);
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
                return appIds.Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IAdaperSetting SetAdaptorSetting(FileTransferSetting fileTransferSetting)
        {
            IAdaperSetting adaptorSettings = new FileTransferSettings();
            adaptorSettings.Host = !string.IsNullOrEmpty(fileTransferSetting.Host)? fileTransferSetting.Host.Trim():"";          
            adaptorSettings.UserName = !string.IsNullOrEmpty(fileTransferSetting.UserName)? fileTransferSetting.UserName.Trim():""                ;

            if (fileTransferSetting.Password !=null && !string.IsNullOrEmpty(fileTransferSetting.Password.Trim()))
            {
                //TODO
                adaptorSettings.Password = fileTransferSetting.Password ;//clientService.DecodeFrom64(fileTransferSetting.Password);
            }          
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

        private bool InstanceLimitAvailable(int appId, int procComponentId, int limit)
        {
            Boolean result = false;
            try
            {
                result = _procSessionRepository.CheckProcSessionLimit(limit, procComponentId);
                if (result)
                {
                    result = _procSessionRepository.IsAppRunning(appId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
    }
}
