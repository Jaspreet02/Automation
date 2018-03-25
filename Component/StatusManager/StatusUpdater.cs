using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using BLW.Lib.Log;
using System.Configuration;
using DbHander;

namespace ApplicationStatusManager
{
    public class StatusUpdater
    {
        IRunDetailsRepository _runNumberRepository = new RunDetailsRepository();
        IRunComponentStatusRepository _runComponentRepository = new RunComponentStatusRepository();
        IApplicationRepository _applicationRepository = new ApplicationRepository();
        ITriggerandStatusFileRepository _triggerStatusRepository = new TriggerandStatusFilesRepository();

        #region---------------  service dependent-----------------------

        /// <summary>
        /// Update Status from status file in database table RunComponentStatus 2 for error ,3 for Complete.
        /// </summary>
        /// <param name="StatusFilePath"></param>
        public void UpdateStatus(string StatusFilePath)
        {
            SingletonLogger.Instance.Debug("Component started for status update for the status files at " + StatusFilePath);
            if (!Directory.Exists(StatusFilePath))
            {
                throw new Exception(StatusFilePath + " status directory does Not Exists.");
            }

            var statusFiles = Directory.GetFiles(StatusFilePath, "status*.xml");
            if (statusFiles.Count() == 0)
            {
                SingletonLogger.Instance.Debug("No Status file found at '" + StatusFilePath + "' location.");
                return;
            }

            SingletonLogger.Instance.Debug(" Total " + statusFiles.Count() + " status files found for processing.");
            foreach (var statusFile in statusFiles)
            {
                try
                {
                    SingletonLogger.Instance.Debug("Status file : " + statusFile);
                    int runComponentId;
                    string status, runNumber, application, componentName, client, errorDetail, endDate;
                    GetStatusInfoFromStatusFile(statusFile, out  runComponentId, out status, out runNumber, out client, out componentName, out application, out errorDetail, out endDate);
                    var runnumberId = _runNumberRepository.GetRunNumberIdByRunNumber(runNumber);
                    SingletonLogger.Instance.Debug("Run Number ID : " + runnumberId);
                    SendStatusEmail statusEmail = new SendStatusEmail();
                    if (status.ToUpper() == "ERROR")
                    {
                        SingletonLogger.Instance.Debug("In error mail block");
                        //If this is a optional component then update Job Status Ready to run
                        if (GetAppComponentTypeByRunNumberNComponentId(runNumber, runComponentId))
                        {
                            //Adding new Status for Optional component (4)
                            UpdateComponentStatusById(runComponentId, ComponentStatusType.Optional, endDate, errorDetail);
                            SingletonLogger.Instance.Debug("update ComponentStatus in RunComponentStatus table to OptionalErrored");
                            UpdateRunStatusByRunNumberId(runnumberId, (byte)RunNumberStatusType.Running);   // Status 1 mean step ready to run
                            SingletonLogger.Instance.Debug("update RunStatus in RunNumberDetails table to 1");
                        }
                        else
                        {
                            UpdateComponentStatusById(runComponentId, ComponentStatusType.Error, endDate, errorDetail);
                            SingletonLogger.Instance.Debug("update ComponentStatus in RunComponentStatus table to Error");
                            UpdateRunStatusByRunNumberId(runnumberId, (byte)RunNumberStatusType.Error);   // Status 3 mean step error out
                            SingletonLogger.Instance.Debug("update RunStatus in RunNumberDetails table to 3");
                        }
                        if (ConfigurationManager.AppSettings["ErrorMail"] == "True")
                        {
                            SingletonLogger.Instance.Debug("Error Mail settings are True");
                            SingletonLogger.Instance.Debug("Send email...");
                            statusEmail.SendInputFileEmail(runComponentId, componentName, runNumber, "{{" + EmailKeyword.COMPONENT_ERROR + "}}",errorDetail);
                        }
                    }
                    else if (status.ToUpper() == "SUCCESS")
                    {
                        SingletonLogger.Instance.Debug("In success mail block");
                        UpdateComponentStatusById(runComponentId, ComponentStatusType.Completed, endDate, errorDetail);
                        UpdateRunStatusByRunNumberId(runnumberId, (byte)RunNumberStatusType.Ready);   // Status 1 mean step ready to run
                        SingletonLogger.Instance.Debug("update RunStatus in RunNumberDetails table to 1");
                        if (ConfigurationManager.AppSettings["SuccessMail"] == "True")
                        {
                            SingletonLogger.Instance.Debug("Success Mail settings are True");
                            SingletonLogger.Instance.Debug("Send email...");
                            statusEmail.SendInputFileEmail(runComponentId, componentName, runNumber,"{{" + EmailKeyword.COMPONENT_SUCCESS + "}}");
                        }
                    }
                    else
                    {
                        SingletonLogger.Instance.Error("Status value is not provided in status file." + statusFile + ". Invalid status file.");
                        continue;
                    }
                    try
                    {
                        // Move status file to backup.
                        var statusBackUpDir = Path.Combine(new FileInfo(statusFile).Directory.FullName, "Back");
                        MoveFile(statusFile, statusBackUpDir, componentName);
                        SingletonLogger.Instance.Debug("Status file has been moved to " + statusBackUpDir + " Directory");
                    }
                    catch (Exception ex)
                    {
                        SingletonLogger.Instance.Error("Error while moving Status file to Status Directory." + ex.ToString());
                    }
                }
                catch (Exception ex)
                {
                    SingletonLogger.Instance.Error(ex.Message);
                }
            }
        }       
 
        /// <summary>
        /// Method:GetStatusInfoFromStatusFile
        /// Description:Get Status info from status file.
        /// </summary>
        /// <param name="statusFileName">File Path</param>
        /// <param name="componentId">Component Id</param>
        /// <param name="status">Status</param>
        /// <param name="runNumber">Run Number</param>
        private void GetStatusInfoFromStatusFile(string statusFileName, out int componentId, out string status, out string runNumber, out string client, out string componentName, out string application, out string errorDetail, out string endDate)
        {
            try
            {
                XDocument xdoc = XDocument.Load(statusFileName);
                componentId = Convert.ToInt32(xdoc.Descendants("RunDetail").Select(x => x.Element("RunComponentStatusId").Value).SingleOrDefault());
                status = xdoc.Descendants("ComponentStatus").Select(x => x.Element("Status").Value).SingleOrDefault().ToString();
                runNumber = xdoc.Descendants("RunDetail").Select(x => x.Element("RunNumber").Value).SingleOrDefault().ToString();
                componentName = xdoc.Descendants("ComponentDetail").Select(x => x.Element("ComponentName").Value).SingleOrDefault();
                client = xdoc.Descendants("Trigger").Select(x => x.Element("ClientName").Value).SingleOrDefault().ToString();
                application = xdoc.Descendants("Trigger").Select(x => x.Element("AppName").Value).SingleOrDefault().ToString();
                errorDetail = xdoc.Descendants("ComponentStatus").Select(x => x.Element("ErrorDescription").Value).SingleOrDefault().ToString();
                endDate = xdoc.Descendants("ComponentStatus").Select(x => x.Element("ComponentEndDate").Value).SingleOrDefault().ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
        /// <summary>
        /// Update Component status by RunComponentId in 'RunComponentStatus' table
        /// </summary>
        /// <param name="Id">RunComponentId</param>
        /// <param name="updatedValue">Status Value</param>
        public void UpdateComponentStatusById(int Id, ComponentStatusType updatedValue, string endDate,string message)
        {
            try
            {
                //calling  the service contract.
                var runComponentStatusInfo = _runComponentRepository.Find(Id);
                runComponentStatusInfo.ComponentStatusId = (byte)updatedValue;
                runComponentStatusInfo.EndDate = DateTime.Parse(endDate);
                runComponentStatusInfo.Message = message;
                _runComponentRepository.Save(runComponentStatusInfo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occur while updating run component status in 'RunComponentStatus' table." + ex.Message);
            }
        }

        /// <summary>
        /// Updating Status in RunDetail table
        /// </summary>
        /// <param name="runNumberId">Run number ID</param>
        /// <param name="updatedValue">Status</param>
        public void UpdateRunStatusByRunNumberId(int runNumberId, byte updatedValue)
        {
            try
            {
                // If all steps of Job are completed then complete the Job
                if (!_runComponentRepository.GetRunComponentStatusbyRunId(runNumberId).Any(x=> x.ComponentStatusId != (int)ComponentStatusType.Completed))
                    updatedValue = (byte)RunNumberStatusType.Completed;
                var result = _runNumberRepository.Find(runNumberId);
                result.RunNumberStatusId = updatedValue;
                _runNumberRepository.Save(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occur while updating status in 'RunDetail' table.", ex);
            }
        }

        /// <summary>
        /// Move or copy Input file to output location ,if file already exists then rename the existing file.
        /// </summary>
        /// <param name="inputfile"></param>
        /// <param name="outputPath"></param>
        /// <param name="componentName"></param>
        /// <param name="isCopy"></param>
        public void MoveFile(string inputfile, string outputPath, string componentName, bool isCopy = false)
        {
            try
            {
                if (!Directory.Exists(outputPath))
                    Directory.CreateDirectory(outputPath);
                string newFile = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(inputfile) + "--" + componentName + Path.GetExtension(inputfile));
                if (File.Exists(newFile))
                {
                    Random objRandon = new Random();
                    var random = objRandon.Next(9999);
                    newFile = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(inputfile) + "--" + componentName + random + Path.GetExtension(inputfile));
                }
                SingletonLogger.Instance.Debug("Source File : " + inputfile);
                SingletonLogger.Instance.Debug("Destination File : " + newFile);
                if (isCopy)
                    File.Copy(inputfile, newFile);
                else
                    File.Move(inputfile, newFile);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while moving status file in archive directory.", ex);
            }
        }

        /// <summary>
        /// Get Application component type by Run number & Component id
        /// </summary>
        /// <param name="runNumber">Run Number</param>
        /// <param name="componentId">Component Id</param>
        /// <returns>if true then component is optional, otherwise mandatory</returns>
        public bool GetAppComponentTypeByRunNumberNComponentId(string runNumber, int componentId)
        {
            try
            {
                //gets the ApplicationId By Run Number.
                var appId = _runNumberRepository.GetApplicationIdByRunNumber(runNumber);
                var isOptionalComp = _runNumberRepository.GetAppComponentTypeByRunCompStatustId(componentId, appId);
                return isOptionalComp;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error occur while processing getting application type (e.g. Optional or Mandatory) [Run number] : {0} & Component Id : {1}.", runNumber, componentId), ex);
            }
        }

        public List<TriggerandStatusFile> GetAllActiveComponentTriggerAndStatusFileLocation()
        {
            try
            {
                //gets the ApplicationId By Run Number.
                return _triggerStatusRepository.FindAllActive().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}