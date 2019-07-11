using System;
using System.Linq;
using BLW.Lib.Log;
using BLW.Lib.SmtpEmail;
using DbHander;

namespace EmailSenderApp
{
    class Processing
    {
        #region Properties

        SmtpDetail smtpDetail = null;
        SmtpMail smtpMail = null;
        IEmailTrackingRepository emailTrackingRepository = null;
        ISmtpDetailsRepository smtpDetailsRepository = null;
        IRunDetailsRepository runDetailsRepository = null;

        #endregion

        #region Methods

        public Processing()
        {
            emailTrackingRepository = new EmailTrackingRepository();
            smtpDetailsRepository = new SmtpDetailsRepository();
            runDetailsRepository = new RunDetailsRepository();
        }

        public void SendEmails()
        {
            try
            {
                var emailDetail = emailTrackingRepository.FindAll().Where(x=> x.EmailStatus.Equals((int)EmailStatusType.Ready)).OrderBy(x=> x.CreatedAt).Take(10).ToList();
                SingletonLogger.Instance.Debug("Ready Emails are :" + emailDetail.Count());

                smtpDetail = smtpDetailsRepository.Find(1);//TODO static value replace later
                SingletonLogger.Instance.Debug("Email Send From " + smtpDetail.SmtpUser);
                smtpMail = new SmtpMail(smtpDetail.SmtpHost, smtpDetail.SmtpUser, smtpDetail.Password);
                foreach (var email in emailDetail)
                {
                    RunDetail runDetail = runDetailsRepository.Find(email.RunNumberId);
                    SingletonLogger.Instance.Debug("Email Id = " + email.EmailTrackingId);
                    if (string.IsNullOrEmpty(email.EmailToIds))
                    {
                        SingletonLogger.Instance.Debug("To email Ids not found in DB.");
                        continue;
                    }
                    SingletonLogger.Instance.Debug("Email Sending To " + email.EmailToIds);
                    smtpMail.To = email.EmailToIds;
                    smtpMail.From = smtpDetail.SmtpUser;
                    smtpMail.Subject = email.Subjects.Replace("{{RUN_NUMBER}}",runDetail.RunNumber);
                    smtpMail.Body = email.Body.Replace("{{RUN_NUMBER}}", runDetail.RunNumber);
                    if (!string.IsNullOrEmpty(email.EmailCcIds))
                        smtpMail.Cc = email.EmailCcIds;

                    try
                    {
                        SingletonLogger.Instance.Debug("Start Sending Email");
                        var result = smtpMail.SendEmail();
                        if (string.IsNullOrEmpty(result))
                        {
                            email.EmailStatus = (int)EmailStatusType.Success;
                            SingletonLogger.Instance.Debug("Email Status Set to SUCCESS");
                            email.SentMessage = "Email sent successfully.";
                            SingletonLogger.Instance.Debug("Email sent successfully");
                        }
                        else
                        {
                            SingletonLogger.Instance.Error("Error in sending email. Detail : " + result);
                            email.EmailStatus = (int)EmailStatusType.Error;
                            email.SentMessage = result;
                        }
                    }
                    catch (Exception ex)
                    {
                        SingletonLogger.Instance.Error("Error in sending email. Detail : " + ex.ToString());
                        email.EmailStatus = (int)EmailStatusType.Error;
                        email.SentMessage = ex.Message;
                    }
                    //update entry
                    SingletonLogger.Instance.Debug("Updating Status in Email Tracking table with Id = " + email.EmailTrackingId);
                    email.Status = true;
                    email.SentDate = DateTime.Now;
                    emailTrackingRepository.Save(email);
                    SingletonLogger.Instance.Debug("Updated Status " + email.Status + " in Email Tracking table successfully");
                }
                SingletonLogger.Instance.Debug("Email component Complete");
            }
            catch (Exception ex)
            {
                SingletonLogger.Instance.Error(ex.ToString());
            }
        }

        #endregion
    }
}
