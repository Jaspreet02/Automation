using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using BLW.Lib.Log;
using BLW.Lib.CoreUtility.Exceptions;
using DbHander;

namespace TriggerManager
{
    class TriggerFileGenerator
    {
        ITriggerandStatusFileRepository _triggerandStatusFileRepository;
        IRunComponentStatusRepository _runComponentRepository;
        IApplicationRepository _applicationRepository;
        IClientRepository _clientRepository;
        IComponentRepository _componentRepository;
        IComponentInputLocationRepository _inputLocationRepository;
        IComponentOutputLocationRepository _outputLocationRepository;
        public TriggerFileGenerator()
        {
            _runComponentRepository = new RunComponentStatusRepository();
            _triggerandStatusFileRepository = new TriggerandStatusFilesRepository();
            _componentRepository = new ComponentRepository();
            _inputLocationRepository = new ComponentInputLocationRepository();
            _outputLocationRepository = new ComponentOutputLocationRepository();
            _clientRepository = new ClientRepository();
            _applicationRepository = new ApplicationRepository();
        }

        #region Class properties
        /// <summary>
        /// Client  Name
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Application  Name
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// RunNumberId
        /// </summary>
        public int RunNumberId { get; set; }
        /// <summary>
        /// ComponentId
        /// </summary>
        public int ComponentId { get; set; }
        /// <summary>
        /// Component Exe path
        /// </summary>
        public string ComponentExe { get; set; }
        /// <summary>
        /// Component Argument
        /// </summary>
        public string ComponentArgment { get; set; }
        /// <summary>
        /// RunNumber
        /// </summary>
        public string RunNumber { get; set; }
        /// <summary>
        /// Step Id
        /// </summary>
        public int RunComponentStatusId { get; set; }
        /// <summary>
        /// Step Name
        /// </summary>
        public string ComponentName { get; set; }
        /// <summary>
        /// StepStatusFileLocation
        /// </summary>
        public string StepStatusFileLocation { get; set; }
        /// <summary>
        /// TriggerFileLocation
        /// </summary>
        public string TriggerFileLocation { get; set; }
        /// <summary>
        /// StepInputLocation list
        /// </summary>
        public List<string> StepInputLocationAndMaskList { get; set; }
        /// <summary>
        /// Configuration Path
        /// </summary>
        public List<string> StepOutputPathAndMaskList { get; set; }
        /// <summary>
        /// StepConfigXmlFilePath
        /// </summary>
        public string StepConfigXmlFilePath { get; set; }
        #endregion

        # region Intilize all used objects

        List<TriggerFileGenerator> TriggerFileGeneratorList = new List<TriggerFileGenerator>();
   

        #endregion

     

        /// <summary>
        /// Method      :   GetTriggerFileGeneratorList.
        /// Description :   Get All component detail for  which trigger file to be generated for  Any Run number
        /// </summary>
        /// <param name="runNumber"></param>
        /// <returns>List of TriggerFileGenerator</returns>
        public List<TriggerFileGenerator> GetTriggerFileGeneratorList(RunDetail runNumber)
        {
         
            // Get ComponentOrder to get all Component for trigger generation
            TriggerFileGeneratorList = new List<TriggerFileGenerator>();
            int minComponentOrder = _runComponentRepository.GetRunComponentStatusbyRunId(runNumber.RunDetailId).Where(x=> x.ComponentStatusId.Equals((byte)ComponentStatusType.Ready)).OrderBy(x=> x.ComponentOrder).Select(x => x.ComponentOrder).FirstOrDefault();
            SingletonLogger.Instance.Debug("ComponentOrder: " + minComponentOrder);

            if (minComponentOrder > 0)
            {
                // get all Component of a particular order for which trigger file will be generated.
                SingletonLogger.Instance.Debug("Getting all Component of " + minComponentOrder + " order");
                var RunComponentStatusList = _runComponentRepository.FindAllActive().Where(x => x.ComponentStatusId == 0 && x.ComponentOrder == minComponentOrder && x.RunNumberId == runNumber.RunDetailId);

                if (RunComponentStatusList.Count() > 0)
                {
                    SingletonLogger.Instance.Debug("Total " + RunComponentStatusList.Count() + " Component found for trigger generation.");
                    #region Process for each component trigger and create TriggerFileGenerator object that will Contains all trigger file detail.
                    foreach (var RunComponentStatus in RunComponentStatusList)
                    {
                        try
                        {
                            ////
                            TriggerandStatusFile objStepStatusAndTriggerLocation = _triggerandStatusFileRepository.FindbyComponentId(RunComponentStatus.ComponentId);
                            if (objStepStatusAndTriggerLocation == null)
                            {
                                throw new Exception(" StepStatusAndTriggerLocation not found for ComponentId " + RunComponentStatus.ComponentId);
                            }

                            var clientApplication = _applicationRepository.Find(runNumber.ApplicationId);

                            if (clientApplication == null)
                            {
                                throw new Exception(" client Application not found for applicationId " + runNumber.ApplicationId);
                            }

                            #region set objTriggerFileGenerator

                            TriggerFileGenerator objTriggerFileGenerator = new TriggerFileGenerator();
                            objTriggerFileGenerator.ClientName =_clientRepository.Find(clientApplication.ClientId).Name;
                            objTriggerFileGenerator.AppName = clientApplication.Name;
                            objTriggerFileGenerator.RunNumber = runNumber.RunNumber;
                            objTriggerFileGenerator.RunComponentStatusId = RunComponentStatus.RunComponentStatusId;
                            objTriggerFileGenerator.ComponentName = _componentRepository.Find(RunComponentStatus.ComponentId).Name;
                            objTriggerFileGenerator.StepStatusFileLocation = objStepStatusAndTriggerLocation.StepStatusLocation;
                            objTriggerFileGenerator.TriggerFileLocation = objStepStatusAndTriggerLocation.TriggerFilelocation;
                            objTriggerFileGenerator.FillInputLocations(RunComponentStatus.ComponentId, clientApplication.ApplicationId);
                            objTriggerFileGenerator.FillOutputLocations(RunComponentStatus.ComponentId, clientApplication.ApplicationId);
                            objTriggerFileGenerator.StepConfigXmlFilePath = "";
                            objTriggerFileGenerator.RunNumberId = runNumber.RunDetailId;
                            objTriggerFileGenerator.ComponentId = RunComponentStatus.ComponentId;
                            objTriggerFileGenerator.ComponentExe = _componentRepository.Find(RunComponentStatus.ComponentId).ComponentExe;
                            #endregion
                            TriggerFileGeneratorList.Add(objTriggerFileGenerator);

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    #endregion
                }
                else
                    SingletonLogger.Instance.Debug(" No Component exists for run number " + runNumber.RunNumber);

            }
            else
                SingletonLogger.Instance.Debug(" No Component exists for run number " + runNumber.RunNumber);


            return TriggerFileGeneratorList;
        }
        /// <summary>
        /// Method:FillOutputLocations
        /// Description:Get all Output Locations for a component from database (ComponentOutputLocations table) .
        /// </summary>
        /// <param name="componentId"></param>
        /// <param name="clientApplicationid"></param>
        public void FillOutputLocations(int componentId, int clientApplicationid)
        {


            var componentOutputLocationList = _outputLocationRepository.GetOutputLocations(clientApplicationid, componentId);
            if (componentOutputLocationList.Count() > 0)
            {
                this.StepOutputPathAndMaskList = new List<string>();
                foreach (var componentOutputLocation in componentOutputLocationList)
                {
                    this.StepOutputPathAndMaskList.Add(componentOutputLocation.TagName + "|" + componentOutputLocation.FileMask + "|" + componentOutputLocation.OutputLocation);

                }
            }
        }
        /// <summary>
        /// Method:FillInputLocations
        /// Description:Get all Input Locations for a component from database (ComponentInputLocation table) .
        /// </summary>
        /// <param name="componentId"></param>
        /// <param name="clientApplicationid"></param>
        public void FillInputLocations(int componentId, int clientApplicationid)
        {

            var ComponentInputLocationList =_inputLocationRepository.GetInputLocations(clientApplicationid,componentId);
            if (ComponentInputLocationList.Count() > 0)
            {
                this.StepInputLocationAndMaskList = new List<string>();
                foreach (var ComponentInputLocation in ComponentInputLocationList)
                {
                    this.StepInputLocationAndMaskList.Add(ComponentInputLocation.TagName + "|" + ComponentInputLocation.FileMask + "|" + ComponentInputLocation.InputLocation);

                }
            }
        }

        //
        //public Int32 GetMinComponentOrderByComponentStatusAndRunNumber(int componentStatus, int runNumberId)
        //{
        //    return dataBaseContext.RunComponentStatus.Where(x => x.ComponentStatus == componentStatus && x.RunNumberId == runNumberId).OrderBy(x => x.ComponentOrder).Select(x => x.ComponentOrder).FirstOrDefault();

        //}

        //public List<RunComponentStatusInfo> GetAllComponentByComponentOrder(int componentOrder, int runNumberId, int componentStatus)
        //{
        //    List<RunComponentStatusInfo> RunComponentStatusList = dataBaseContext.RunComponentStatus.Where(x => x.ComponentStatus == componentStatus && x.ComponentOrder == componentOrder && x.RunNumberId == runNumberId).Select(x => x).ToList<RunComponentStatusInfo>();
        //    return RunComponentStatusList;
        //}
       //
        //public Application GetApplicationByAppId(int AppId)
        //{
        //    try
        //    {
        //        var application = dataBaseContext.Applications.Where(x => x.ID == AppId && x.IsDeleted == false).Select(x => x).SingleOrDefault();
        //        return application;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public ComponentTriggerAndStatusFile GetComponentTriggerAndStatusFileLocationByComponentId(int componentId)
        //{
        //    return dataBaseContext.ComponentTriggerAndStatusFiles.Where(x => x.ComponentId == componentId && x.IsActive == 1).Select(x => x).FirstOrDefault();
        //}

        
    

        //public List<ComponentOutputLocationInfo> GetAllOutputLocationbyComponentAndAppId(int componentId, int clientApplicationid)
        //{
        //    List<ComponentOutputLocation> ComponentOutputLocationList = dataBaseContext.ComponentOutputLocations.Where(x => x.ApplicationsId == clientApplicationid && x.ComponentsId == componentId && x.IsActive == 1).ToList<ComponentOutputLocation>();
        //    return ComponentOutputLocationList;
        //}


        //public List<ComponentInputLocation> GetAllInputLocationbyComponentAndAppId(int componentId, int clientApplicationid)
        //{
        //    List<ComponentInputLocation> ComponentInputLocationList = dataBaseContext.ComponentInputLocations.Where(x => x.ApplicationsId == clientApplicationid && x.ComponentsId == componentId && x.IsActive == 1).ToList<ComponentInputLocation>();
        //    return ComponentInputLocationList;
        //}

        /// <summary>
        /// Create Trigger file
        /// </summary>
        /// <param name="objTriggerFileGenerator"></param>
        public void CreateTriggerFile(TriggerFileGenerator objTriggerFileGenerator)
        {
            if (objTriggerFileGenerator == null)
                throw new InvalidDataException("Trigger File");

            string triggerFilePath = objTriggerFileGenerator.TriggerFileLocation + "\\" + objTriggerFileGenerator.RunNumber + ".xml";
            try
            {
                if (!Directory.Exists(objTriggerFileGenerator.TriggerFileLocation))
                    Directory.CreateDirectory(objTriggerFileGenerator.TriggerFileLocation);

                // Open the file just specified such that no one else can use it.                
                using (FileStream fs = File.Open(triggerFilePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.WriteLine("<?xml version=" + '"' + "1.0" + '"' + " encoding=" + '"' + "utf-8" + '"' + "?>");
                        writer.WriteLine("<Trigger>");
                        writer.WriteLine("<ClientName>" + objTriggerFileGenerator.ClientName + "</ClientName>");
                        writer.WriteLine("<AppName>" + objTriggerFileGenerator.AppName + "</AppName>");                    
                        writer.WriteLine("<RunDetail>");
                        writer.WriteLine("<RunNumber>" + objTriggerFileGenerator.RunNumber + "</RunNumber>");
                        writer.WriteLine("<ComponentStatus>");
                        writer.WriteLine("<Status></Status>");
                        writer.WriteLine("<ComponentStartDate></ComponentStartDate>");
                        writer.WriteLine("<ComponentEndDate></ComponentEndDate>");
                        writer.WriteLine("<ErrorDescription></ErrorDescription>");
                        writer.WriteLine("</ComponentStatus>");
                        writer.WriteLine("<RunComponentStatusId>" + objTriggerFileGenerator.RunComponentStatusId + "</RunComponentStatusId>");
                        writer.WriteLine("</RunDetail>");
                        writer.WriteLine("<ComponentDetail>");
                        writer.WriteLine("<ComponentName>" + objTriggerFileGenerator.ComponentName + "</ComponentName>");
                        writer.WriteLine("<ComponentExe>" + objTriggerFileGenerator.ComponentExe + "</ComponentExe>");
                        writer.WriteLine("<ComponentStatusDirectory>" + objTriggerFileGenerator.StepStatusFileLocation + "</ComponentStatusDirectory>");
                        writer.WriteLine("<ComponentInputLocation>");
                        if (objTriggerFileGenerator.StepInputLocationAndMaskList != null)
                        {
                            if (objTriggerFileGenerator.StepInputLocationAndMaskList.Count() > 0)
                            {
                                foreach (var StepInputLocationAndMask in objTriggerFileGenerator.StepInputLocationAndMaskList)
                                {
                                    var StepInputLocationAndMaskArr = StepInputLocationAndMask.Split('|');
                                    var tagName = StepInputLocationAndMaskArr[0];

                                    var fileMask = StepInputLocationAndMaskArr[1];
                                    if (fileMask == "")
                                        fileMask = "*";
                                    var inputLocation = StepInputLocationAndMaskArr[2];
                                    writer.WriteLine("<" + tagName + " Mask=" + '"' + fileMask + '"' + ">" + inputLocation + "</" + tagName + ">");

                                }
                            }
                            else
                            {
                                SingletonLogger.Instance.Debug("No input location found for Component " + objTriggerFileGenerator.ComponentName + " Check the ComponentInputLocations table. ");
                            }
                        }

                        writer.WriteLine("</ComponentInputLocation>");
                        writer.WriteLine("<ComponentOutputLocation>");
                        if (objTriggerFileGenerator.StepOutputPathAndMaskList != null)
                        {
                            if (objTriggerFileGenerator.StepOutputPathAndMaskList.Count() > 0)
                            {
                                foreach (var StepOutputPathAndMask in objTriggerFileGenerator.StepOutputPathAndMaskList)
                                {
                                    var StepOutputPathAndMaskArr = StepOutputPathAndMask.Split('|');
                                    var tagName = StepOutputPathAndMaskArr[0];

                                    var fileMask = StepOutputPathAndMaskArr[1];
                                    if (fileMask == "")
                                        fileMask = "*";
                                    var outputLocation = StepOutputPathAndMaskArr[2];

                                    writer.WriteLine("<" + tagName + " Mask=" + '"' + fileMask + '"' + ">" + outputLocation + "</" + tagName + ">");
                                }
                            }
                            else
                            {
                                SingletonLogger.Instance.Debug("No output location found for Component " + objTriggerFileGenerator.ComponentName + " Check the ComponentOutputLocations table. ");
                            }
                        }
                        writer.WriteLine("</ComponentOutputLocation>");
                        writer.WriteLine("<ComponentConfigXmlFilePath>" + objTriggerFileGenerator.StepConfigXmlFilePath + "</ComponentConfigXmlFilePath>");
                        writer.WriteLine("</ComponentDetail>");
                        writer.Write("</Trigger>");
                    }               
                }
            }           
            catch (IOException iEx)
            {
                throw new BLW.Lib.CoreUtility.Exceptions.IOBLWException("File is locked by other process or already exists.", iEx, triggerFilePath, "Trigger Generator");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method:CreateTriggerFile
        /// Description:Create Trigger file for all component and update ComponentStatus in RunComponentStatus table to 1  reprents running status .
        /// </summary>
        /// <param name="TriggerFileGeneratorList"></param>
        public void CreateTriggerFile(List<TriggerFileGenerator> TriggerFileGeneratorList)
        {
            foreach (var TriggerFileGenerator in TriggerFileGeneratorList)
            {
                #region Creating Trigger file

                try
                {
                    SingletonLogger.Instance.Debug("Creating trigger file for " + TriggerFileGenerator.ComponentName + " Component");
                    CreateTriggerFile(TriggerFileGenerator);
                    SingletonLogger.Instance.Debug(" trigger file for " + TriggerFileGenerator.ComponentName + " Component has been successfully generated at " + TriggerFileGenerator.TriggerFileLocation + " location");
                     var result = _runComponentRepository.Find(TriggerFileGenerator.RunComponentStatusId);
                    result.StartDate = DateTime.Now;
                    result.ComponentStatusId = (int)ComponentStatusType.Running;
                    _runComponentRepository.Save(result);
                    SingletonLogger.Instance.Debug("ComponentStatus for " + TriggerFileGenerator.ComponentName + " Component has been updated to 1 in RunComponentStatus table");
                }              
                catch (IOBLWException)
                {
                    throw;
                }
                catch (Exception exp)
                {
                    throw new Exception("Error occur while process generating trigger file for Run Number = " + RunNumber + " and for Step = " + TriggerFileGenerator.ComponentName, exp);
                }

                #endregion
            }
        }
        /// <summary>
        /// Method:CheckForTriggerFileCreation
        /// Description:Check Wither to create Trigger file or not. if any component for this run number is already running or error out 
        /// then not created the trigger file and continue for other run number trigger file creation.
        /// </summary> 
        /// <param name="runNumber"></param>
        /// <returns>true or false</returns>
        public bool CheckForTriggerFileCreation(RunDetail runNumber)
        {
            //check for running component
            var runningComponents = _runComponentRepository.FindAllActive().Where(x => x.RunNumberId.Equals(runNumber.RunDetailId) && x.ComponentStatusId.Equals((int)ComponentStatusType.Running));
            if (runningComponents.Count() > 0)
            {
                SingletonLogger.Instance.Debug("Trigger file can not be created for run number " + runNumber.RunNumber + " because Previous component is not completed and in running state.");
                return false;
            }

            //check for Error Out component
            var errorOutComponents = _runComponentRepository.FindAllActive().Where(x => x.RunNumberId.Equals(runNumber.RunDetailId) && x.ComponentStatusId.Equals((int)ComponentStatusType.Error));
            if (errorOutComponents.Count() > 0)
            {
                SingletonLogger.Instance.Debug("Trigger file can not be created for run number " + runNumber.RunNumber + " because Previous component is not completed and  it is error out.");
                return false;
            }
            return true;
        }
    }
}

