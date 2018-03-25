using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace BLW.Lib.CoreUtility
{
    public class TriggerFileReader
    {
        public string TriggerFileLocaton { get; set; }
        public string ClientName { get; set; }
        public string ApplicationName { get; set; }
        public string RunNumber { get; set; }
        public string RunType { get; set; }
        public int RunComponentStatusId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentExe { get; set; }
        public string ComponentStatusDirectory { get; set; }
        public string ComponentInputLocation { get; set; }
        public string ComponentOutputLocation { get; set; }
        public string ComponentConfigXmlFilePath { get; set; }
        public List<ComponentInLocation> InputDetails { get; set; }
        public List<ComponentOutLocation> OutputDetails { get; set; }
        FileStream _fs;
        public void Lock()
        {
            try
            {
                _fs = File.Open(TriggerFileLocaton, FileMode.Open, FileAccess.ReadWrite, FileShare.None);               
            }
            catch (Exception)
            {
                /*Do nothing*/
            }
        }
        public void Unlock()
        {
            try
            {
                if (_fs != null)                    
                    _fs.Dispose();                
            }
            catch (Exception)
            {
                /*Do nothing*/
            }
        }
        /// <summary>
        /// Method:GetTriggerFileDetail.
        /// Description:Collects information from the trigger file to TriggerFileReader object.
        /// </summary>
        /// <exception cref="BLW.Lib.CoreUtility.Exceptions.UnauthorizedAccessBLWException">path specified a file that is read-only and access is not Read -or- path specified a directory -or- The caller does not have the required permission.-or-mode is System.IO.FileMode.Create and the specified file is a hidden file.</exception>
        /// <returns>instance of TriggerFileReader.</returns>
        public TriggerFileReader GetTriggerFileDetail()
        {
            TriggerFileReader objTriggerFileReader = new TriggerFileReader();
            try
            {
                using (FileStream fs = File.Open(TriggerFileLocaton, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    XDocument xdocument = XDocument.Load(fs);
                    var allElement = xdocument.Elements("Trigger");
                    var allTag = allElement.First();

                    objTriggerFileReader.ClientName = allTag.Element("ClientName").Value;
                    objTriggerFileReader.ApplicationName = allTag.Element("AppName").Value;

                    // Read run detail Element and Value
                    var runDetailelement = allTag.Element("RunDetail");
                    objTriggerFileReader.RunNumber = runDetailelement.Element("RunNumber").Value;
                    objTriggerFileReader.RunComponentStatusId = Convert.ToInt32(runDetailelement.Element("RunComponentStatusId").Value);

                    // Read run Component Detail element
                    var componentDetailElement = allTag.Element("ComponentDetail");
                    objTriggerFileReader.ComponentName = componentDetailElement.Element("ComponentName").Value;
                    objTriggerFileReader.ComponentExe = componentDetailElement.Element("ComponentExe").Value;
                    objTriggerFileReader.ComponentStatusDirectory = componentDetailElement.Element("ComponentStatusDirectory").Value;
                    objTriggerFileReader.ComponentConfigXmlFilePath = componentDetailElement.Element("ComponentConfigXmlFilePath").Value;

                    List<ComponentInLocation> objInputLocation = new List<ComponentInLocation>();
                    var inputElement = from inputelement in componentDetailElement.Descendants("InputLocation")
                                       select inputelement;

                    if (inputElement.Count() > 0)
                    {
                        foreach (var inputAtribute in inputElement)
                        {
                            ComponentInLocation objInput = new ComponentInLocation();
                            objInput.DirectoryLocation = inputAtribute.Value;
                            objInput.FileMask = inputAtribute.FirstAttribute.Value;
                            objInputLocation.Add(objInput);
                        }

                        objTriggerFileReader.InputDetails = objInputLocation;
                    }

                    // Component Output location
                    List<ComponentOutLocation> objOutputLocation = new List<ComponentOutLocation>();
                    var outputElement = from outputelement in componentDetailElement.Descendants("OutputLocation")
                                        select outputelement;

                    if (outputElement.Count() > 0)
                    {
                        foreach (var outputAtribute in outputElement)
                        {
                            ComponentOutLocation objOutput = new ComponentOutLocation();
                            objOutput.DirectoryLocation = outputAtribute.Value;
                            objOutput.FileMask = outputAtribute.FirstAttribute.Value;
                            objOutputLocation.Add(objOutput);
                        }

                        objTriggerFileReader.OutputDetails = objOutputLocation;
                    }

                    return objTriggerFileReader;
                }
            }
            catch (IOException uEx)
            {
                throw new BLW.Lib.CoreUtility.Exceptions.IOBLWException("File is locked by other process.", uEx, TriggerFileLocaton, "Trigger Reader");
            }
            catch (Exception ex)
            {
                throw new Exception("Error while reading the Trigger file. File Path " + TriggerFileLocaton, ex);
            }

        }
    }
    public class ComponentInLocation
    {
        public string DirectoryLocation { get; set; }
        public string FileMask { get; set; }
    }

    public class ComponentOutLocation
    {
        public string DirectoryLocation { get; set; }
        public string FileMask { get; set; }
    }
}