using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace BLW.Lib.SmtpEmail
{
    public class SmtpMail
    {
        #region SMTP and Email Properties

        private SmtpClient smtp;
        private String smtpUser = string.Empty;
        private String smtpPwd = string.Empty;
        private String smtpHost = string.Empty;
        private NetworkCredential cred;
        private MailMessage mail;

        /// <summary>
        /// Email To
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// Email From
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// Email Subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Email Body
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Attachment File Path
        /// </summary>
        public List<string> ZipFilePath { get; set; }
        /// <summary>
        /// Email CC
        /// </summary>
        public string Cc { get; set; }
        /// <summary>
        /// Email BCC
        /// </summary>
        public string Bcc { get; set; }

        #endregion

        /// <summary>        
        /// Constructor : SmtpMail
        /// Description : SMTP Configuration
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="user">SMTP User Name</param>
        /// <param name="pass">SMTP Password</param>
        public SmtpMail(String host, String user, String pass)
        {
            this.SmtpSettings(host, user, pass);
        }

        /// <summary>
        /// Set properties for SMTP client
        /// </summary>
        /// <param name="smptHost">SMTP Host</param>
        /// <param name="smtpUser">SMTP User</param>
        /// <param name="smtpPassword">SMTP Password</param>
        private void SmtpSettings(string smptHost, string smtpUser, string smtpPassword)
        {
            cred = new NetworkCredential(smtpUser, smtpPassword);
            smtpHost = smptHost;
            smtp = new SmtpClient(smtpHost);
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = cred;
            smtp.Host = smtpHost;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <returns>True/False</returns>
        public string SendEmail()
        {
            MailSettings();
            AddAttachment();
            try
            {
                smtp.Send(mail);
            }
            catch (SmtpException ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        /// <summary>
        /// Function       : MailSettings
        /// Description    : Email setting
        /// </summary>
        private void MailSettings()
        {
            mail = new MailMessage();
            mail.To.Add(To);
            mail.From = new MailAddress(From, From, System.Text.Encoding.UTF8);
            mail.Headers.Add("To", To);
            mail.Headers.Add("Return-Path", From);
            mail.Headers.Add("Subject", Subject);
            if (!string.IsNullOrEmpty(Cc))
            {
                mail.CC.Add(Cc);
                mail.Headers.Add("Cc", Cc);
            }
            if (!string.IsNullOrEmpty(Bcc))
            {
                mail.Bcc.Add(Bcc);
                mail.Headers.Add("Bcc", Bcc);
            }
            mail.Subject = Subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = Body;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
        }

        /// <summary>
        /// Description : Send attachments
        /// </summary>
        private void AddAttachment()
        {
            if (ZipFilePath != null)
            {
                if (ZipFilePath.Count != 0)
                {
                    foreach (var item in ZipFilePath)
                    {
                        smtp.Timeout = 1000000;
                        Attachment attach = new Attachment(item);
                        mail.Attachments.Add(attach);
                    }
                }
            }
        }
    }
}