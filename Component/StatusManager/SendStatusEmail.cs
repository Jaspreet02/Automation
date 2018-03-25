using System;
using BLW.Lib.Log;
using DbHander;
using DbHander.EmailService;

namespace ApplicationStatusManager
{
    public class SendStatusEmail
    {

        IRunDetailsRepository _runNumberRepository = new RunDetailsRepository();
        IRunComponentStatusRepository _runComponentRepository = new RunComponentStatusRepository();
        IApplicationRepository _applicationRepository = new ApplicationRepository();
        EmailService _emailService = new EmailService();
        
        public void SendInputFileEmail(int runComponentId,string component,string runNumber,  string token,string message = "Success")
        {
            try
            {
                SingletonLogger.Instance.Debug("Fetching email template for TOKEN = " + token);
                Application appInfo = _applicationRepository.Find(_runNumberRepository.GetApplicationIdByRunNumber(runNumber));
                int componentId = _runComponentRepository.Find(runComponentId).ComponentId;
                EmailTemplate template = _emailService.GetEmailTemplate(appInfo.ClientId, appInfo.ApplicationId, componentId, token, -1, null, -1);
                if (template != null)
                {
                    EmailTracking tracking = _emailService.ConvertToTracking(template, _runNumberRepository.GetRunNumberIdByRunNumber(runNumber), EmailStatusType.Ready);
                    if (tracking != null)
                    {
                        tracking.SentDate = DateTime.Now;
                        tracking.Body = tracking.Body.Replace("{{CLIENT_NAME}}",appInfo.ClientId.ToString());
                        tracking.Body = tracking.Body.Replace("{{APPLICATION_NAME}}", appInfo.Name).Replace("{{COMPONENT_NAME}}", component);
                        tracking.Body = tracking.Body.Replace("{{MESSAGE}}",message);
                        tracking.Subjects = tracking.Subjects.Replace("{{COMPONENT_NAME}}", component).Replace("{{APPLICATION_NAME}}", appInfo.Name).Replace("{{CLIENT_NAME}}",appInfo.ClientId.ToString());
                        if (!_emailService.SaveEmailTracking(tracking))
                            SingletonLogger.Instance.Error("Error occured while email saving to email tracking.");
                    }
                }
                else
                {
                    SingletonLogger.Instance.Debug("Email Template not found in database for TOKEN = " + token);                               
                }
            }
            catch (Exception ex)
            {
                SingletonLogger.Instance.Error("Error occured while sending email about file download = " + ex.Message);
            }
        }
    }
}
