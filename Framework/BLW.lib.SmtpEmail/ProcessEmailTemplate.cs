using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace BLW.Lib.SmtpEmail
{
    public class ProcessEmailTemplate
    {
        /// <summary>
        /// Function :      GetProcessedContent
        /// Description :   Process email template and merge variables in template
        /// </summary>
        /// <param name="ConfigurationVariables">Value Pass In Email Template</param>
        /// <returns></returns>
        public string GetProcessedContent(Dictionary<string, string> ConfigurationVariables, string template)
        {
            var content = template;            
            foreach (KeyValuePair<string, string> item in ConfigurationVariables)
            {
                content = Regex.Replace(content, item.Key, item.Value);
            }
            return content;
        }

        public string ComponentStatusSuccessTemplate()
        {
            return "<html><head></head><body><html><head></head><body><br />Client : @ClientName<br /><br />Application : @ApplicationName<br /><br />Status update of the application is as follows:-<br /><br />Step : <b>@StepName</b> Completed Successfully for <br /><br /> File : <B>@FileGuid</B><br /></body></html></body></html>";
        }
        

        public string ComponentErrorOutTemplate()
        {
            return "<html><head></head><body><html><head></head><body><br />Client : @ClientName<br /><br />Application : @ApplicationName<br /><br />Status update of the application is as follows:-<br /><br />Step : <b>@StepName</b> error out for <br /><br /> File : <B>@FileGuid</B><br /><br />Error message : @error<br /></body></html></body></html>";
        }
        public string ComponentErrorOutNextReadyToRunTemplate()
        {
            return "<html><head></head><body><html><head></head><body><br />Client : @ClientName<br /><br />Application : @ApplicationName<br /><br />Status update of the application is as follows:-<br /><br />Step : <b>@StepName</b> [Optional] component error out for <br /><br /> File : <B>@FileGuid</B><br /><br />Error message : @error<br /></body></html></body></html>";
        }

        public string UserInfoTemplate()
        {
            return "<html><head></head><body><html><head></head><body><br />User Name : @EmailAddress<br /><br />Password : @Password<br /><br />Change Password Link : @ChangePassword<br /></body></html></body></html>";
        }
    }
}
