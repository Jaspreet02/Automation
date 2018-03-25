using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BLW.Lib.CoreUtility
{
    public class XmlHelper
    {
        /// <summary>
        /// Method:WriteComponentStatusInTriggerFile
        /// Update the status of trigger file
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="componentStartDate">Start date</param>
        /// <param name="componentEndDate">End date</param>
        /// <param name="status">success/error</param>        
        /// <param name="errorDescription"></param>
        public void WriteComponentStatusInTriggerFile(string xmlPath, string componentStartDate, string componentEndDate, string status = "success", string errorDescription = "")
        {
            try
            {
                XDocument xdoc = XDocument.Load(xmlPath);
                var items = from i in xdoc.Descendants("ComponentStatus")
                            select i;

                foreach (var item in items)
                {
                    item.Element("Status").Value = status;
                    item.Element("ComponentStartDate").Value = componentStartDate;
                    item.Element("ComponentEndDate").Value = componentEndDate;
                    item.Element("ErrorDescription").Value = errorDescription;
                }
                xdoc.Save(xmlPath);
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
    }
}
