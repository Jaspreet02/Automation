using System;
using System.Collections.Generic;
using BLW.Lib.Log;
using DbHander.EmailService;
using DbHander;

namespace JobInit
{
    public class SendInputEmail
    {
        EmailService emailService = null;    

        public SendInputEmail()
        {
            emailService = new EmailService();
        }

        public void SendInputFileEmail(Application info,Int32 runNumberID, List<string> emailList)
        {
            try
            {
                SingletonLogger.Instance.Debug("Fetching email template for TOKEN = " + "DOWNLOAD_FILE");
                EmailTemplate template = emailService.GetEmailTemplate(info.ClientId, info.ApplicationId, -1, "{{DOWNLOAD_FILE}}", -1, null, -1);
               
                if (template != null)
                {
                    SingletonLogger.Instance.Debug("templateID  = " + template.EmailTemplateId + " runId " + runNumberID);
                    EmailTracking tracking = emailService.ConvertToTracking(template, runNumberID, EmailStatusType.Ready);
                    SingletonLogger.Instance.Debug("trackingID  = " + tracking.EmailTrackingId);
                    if (tracking != null)
                    {
                        tracking.SentDate = DateTime.Now;
                        tracking.CreatedAt = DateTimeOffset.Now;
                        tracking.Body = tracking.Body.Replace("{{FILE_LIST}}", CreateTable(emailList));
                        tracking.Body = tracking.Body.Replace("{{FILE_NAME}}", CreateTable(emailList));
                        tracking.Subjects = tracking.Subjects.Replace("{{APPLICATION_NAME}}", info.Name);
                        tracking.Body = tracking.Body.Replace("{{APPLICATION_NAME}}", info.Name);
                        if (!emailService.SaveEmailTracking(tracking))
                            SingletonLogger.Instance.Error("Error occured while email saving to email tracking.");
                    }
                }
                else
                {
                    SingletonLogger.Instance.Debug("Email Template not found in database for TOKEN = " + "DOWNLOAD_FILE");                               
                }
            }
            catch (Exception ex)
            {
                SingletonLogger.Instance.Error("Error occured while sending email about file download = " + ex.Message+" "+ex.InnerException!=null?ex.InnerException.Message:""+ex.ToString()+" "+ex.StackTrace);
            }
        }

        private string CreateTable(List<string> itemList)
        {
            string result = string.Empty;
            DateTime currentTime = DateTime.Now;
            if (itemList.Count > 0)
            {
                result = "<table border='1'><tr><th>File Name</th><th>Download Time</th></tr>";
                // Loop over pairs with foreach
                foreach (var item in itemList)
                {
                    result += string.Format("<tr><td>{0}</td><td>{1}</td></tr>", item, currentTime.ToString("dd/MM/yyyy HH:mm:ss tt"));
                }
                result += "</table>";
            }
            return result;
        }
    }
}
