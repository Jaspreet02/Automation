using System;
using BLW.Lib.Log;
using DbHander;

namespace ApplicationStatusManager
{
    public class SendStatusEmail
    {

        IRunDetailsRepository _runNumberRepository = new RunDetailsRepository();
        IRunComponentStatusRepository _runComponentRepository = new RunComponentStatusRepository();
        IApplicationRepository _applicationRepository = new ApplicationRepository();
        IEmailTrackingRepository    _emailTrackingRepository = new EmailTrackingRepository();
        IEmailTemplateRepository _emailTemplateRepository = new EmailTemplateRepository();
        
        public void SendInputFileEmail(int runComponentId,string component,string runNumber,  string token,string message = "Success")
        {
            try
            {
                SingletonLogger.Instance.Debug("Fetching email template for TOKEN = " + token);
                Application appInfo = _applicationRepository.Find(_runNumberRepository.GetApplicationIdByRunNumber(runNumber));
                int componentId = _runComponentRepository.Find(runComponentId).ComponentId;
                EmailTemplate template = _emailTemplateRepository.EmailTemplate(x=> x.ClientId == appInfo.ClientId && x.ApplicationId == appInfo.ApplicationId &&  x.ApplicationComponentId == componentId && x.EmailToken == token);
                if (template != null)
                {
                   EmailTracking tracking = new EmailTracking()
                    {
                        RunNumberId = _runNumberRepository.GetRunNumberIdByRunNumber(runNumber),
                        FromEmailId = template.EmailFromSmtpId.ToString(),
                        EmailTemplateId = template.EmailTemplateId,
                        Subjects = template.Subject,
                        Body = template.Body,
                        EmailStatus = (int)EmailStatusType.Ready
                    };

                    if (tracking != null)
                    {
                        tracking.SentDate = DateTime.Now;
                        tracking.Body = tracking.Body.Replace("{{CLIENT_NAME}}",appInfo.ClientId.ToString());
                        tracking.Body = tracking.Body.Replace("{{APPLICATION_NAME}}", appInfo.Name).Replace("{{COMPONENT_NAME}}", component);
                        tracking.Body = tracking.Body.Replace("{{MESSAGE}}",message);
                        tracking.Subjects = tracking.Subjects.Replace("{{COMPONENT_NAME}}", component).Replace("{{APPLICATION_NAME}}", appInfo.Name).Replace("{{CLIENT_NAME}}",appInfo.ClientId.ToString());
                        if (_emailTrackingRepository.Save(tracking) != 0)
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
