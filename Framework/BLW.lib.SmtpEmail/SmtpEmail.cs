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

        private SmtpClient _smtp;
        private NetworkCredential _credential;
        private MailMessage _mail;

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
            _credential = new NetworkCredential(smtpUser, smtpPassword);
            _smtp = new SmtpClient(smptHost, 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = _credential,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
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
                _smtp.Send(_mail);
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
            _mail = new MailMessage();
            _mail.To.Add(To);
            _mail.From = new MailAddress(From, From, System.Text.Encoding.UTF8);
            _mail.Headers.Add("To", To);
            _mail.Headers.Add("Return-Path", From);
            _mail.Headers.Add("Subject", Subject);
            if (!string.IsNullOrEmpty(Cc))
            {
                _mail.CC.Add(Cc);
                _mail.Headers.Add("Cc", Cc);
            }
            if (!string.IsNullOrEmpty(Bcc))
            {
                _mail.Bcc.Add(Bcc);
                _mail.Headers.Add("Bcc", Bcc);
            }
            _mail.Subject = Subject;
            _mail.SubjectEncoding = Encoding.UTF8;
            _mail.Body = Body;
            _mail.BodyEncoding = Encoding.UTF8;
            _mail.IsBodyHtml = true;
            _mail.Priority = MailPriority.High;
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
                        _smtp.Timeout = 1000000;
                        Attachment attach = new Attachment(item);
                        _mail.Attachments.Add(attach);
                    }
                }
            }
        }
    }
}