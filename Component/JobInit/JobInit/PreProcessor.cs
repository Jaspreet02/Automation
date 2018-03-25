using BLW.Lib.CoreUtility;
using BLW.Lib.Log;
using DbHander;
using System;
using System.IO;
using System.Linq;

namespace JobInit
{
    class PreProcessor
    {
        PatternMatchingMapper mapper = new PatternMatchingMapper();
        IApplicationComponentRepository objApplicationComponentRepository = new ApplicationComponentRepository();
        IRunDetailsRepository objRunDetailsRepository = new RunDetailsRepository();
        IRunComponentStatusRepository objRunComponentStatusRepository = new RunComponentStatusRepository();

        /// <summary>
        ///  Method:ProcessRunNumber
        ///  Description:This initialize the components step for each Run Number.
        ///  </summary>
        /// <param name="objRunNumberDetail"></param>
        public void ProcessRunNumber(RunDetail runDetail, string fileDownloadLocation)
        {
            var runNumberDirectory = fileDownloadLocation;
            if (!Directory.Exists(runNumberDirectory))
            {
                SingletonLogger.Instance.Debug("  Directory " + runDetail.RunNumber + " Does not Exists in " + runNumberDirectory);
                return;
            }
            var inputfiles = Directory.GetFiles(runNumberDirectory);
            if (inputfiles.Count() == 0)
            {
                SingletonLogger.Instance.Debug(" No Files exists to process under Run number ." + runNumberDirectory);
                return;
            }


            var runNumber = runDetail.RunNumber;
            var appId = runDetail.ApplicationId;


            SingletonLogger.Instance.Debug("appId " + appId);

            //Get All Component of particulate Application

            // List<ApplicationComponent> applicationComponentList = objApplicationComponentsDal.GetAllApplicationComponentsByAppId(appId);
            var applicationComponentList = objApplicationComponentRepository.GetApplicationComponentListbyappId(appId);

            if (applicationComponentList.Count() == 0)
            {
                throw new Exception(" No Component steps exists for Application Id = " + appId);
            }
            else
            {
                try
                {
                    SingletonLogger.Instance.Debug("Total " + applicationComponentList.Count() + " Component steps founds .");
                    //Get RunNumberId By RunNumber From RunNumberDetails Table.
                    int runNumberId = objRunDetailsRepository.GetRunDetailByRunNumber(runNumber).RunDetailId;
                    SingletonLogger.Instance.Debug("runNumberId " + runNumberId);

                    //Make entry in RunComponentStatus table For Each Step For a RunNumber
                    foreach (var applicationComponent in applicationComponentList)
                    {
                        RunComponentStatus entity = new RunComponentStatus();
                        entity.RunNumberId = runNumberId;
                        entity.ComponentId = applicationComponent.ComponentId;
                        entity.ComponentOrder = applicationComponent.ComponentOrder;
                        entity.ComponentStatusId = 0;
                        entity.EndDate = null;
                        entity.StartDate = null;
                        objRunComponentStatusRepository.Save(entity);
                    }
                    SingletonLogger.Instance.Debug("All Component entry has been Added to RunComponentStatus table  for RunNumber " + runNumber);

                    // Update RunStatus in RunNumberDetails to 1
                    objRunDetailsRepository.UpdateRunStatusByRunNumberId(runNumberId, (byte)RunNumberStatusType.Ready);
                    SingletonLogger.Instance.Debug("RunStatus has been updated RunNumberDetails table for RunNumber " + runNumber);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
