using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace BLW.Lib.CoreUtility
{
    public class ComponentInputFiles
    {
        /// <summary>
        /// Method:GetComponentInfo
        /// Description:Get information of all triggers files of a component.
        /// </summary>
        /// <param name="componentName">componentName</param>
        /// <returns>List of TriggerFileReader </returns>
        public List<TriggerFileReader> GetComponentTriggerFilesDetails(string componentName)
        {
            try
            {
                #region Get Componenet id by componentName

                ComponentsDal objComponentsDal = new ComponentsDal();
                ComponentTriggerAndStatusFilesDal objComponentTriggerAndStatusFilesDal = new ComponentTriggerAndStatusFilesDal();
                int ComponentId = objComponentsDal.GetComponentIdByComponentName(componentName);
                //Get Trigger File Location
                ComponentTriggerAndStatusFile objComponentTriggerAndStatusFile = objComponentTriggerAndStatusFilesDal.GetComponentTriggerAndStatusFileLocationByComponentId(ComponentId);
                
                #endregion

                if (objComponentTriggerAndStatusFile != null)
                {
                    TriggerFileReader objTriggerFileReader = new TriggerFileReader();
                    //
                    if (!Directory.Exists(objComponentTriggerAndStatusFile.TriggerFilelocation))
                        throw new Exception("Trigger directory not foud for Component " + componentName);

                    //Get all trigger files
                    var triggerFiles = Directory.GetFiles(objComponentTriggerAndStatusFile.TriggerFilelocation, "*.xml");
                    if (triggerFiles.Count() > 0)
                    {
                        List<TriggerFileReader> triggerFileReaderList = new List<TriggerFileReader>();

                        #region get information for all trigger file and add to TriggerFileReader list
                        foreach (var triggerFile in triggerFiles)
                        {
                            objTriggerFileReader.TriggerFileLocaton = triggerFile;
                            var triggerDetail = objTriggerFileReader.GetTriggerFileDetail();
                            Console.WriteLine("Process successfully read trigger XML file " + triggerFile);
                            triggerFileReaderList.Add(triggerDetail);
                        }
                        #endregion

                        return triggerFileReaderList;
                    }
                    else
                        throw new Exception(" No Trigger File exists on location  " + objComponentTriggerAndStatusFile.TriggerFilelocation + "  for Component " + componentName + "  in database.");
                  
                }
                else
                    throw new Exception("Trigger location not foud for Component " + componentName + " in database.");
                
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
