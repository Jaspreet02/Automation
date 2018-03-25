
using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MobileService
{
    public class SendEmailService : ISendEmailService
    {
        public bool SendEmail(IdentityMessage message)
        {
            DeliverAsync(message.Destination, message.Subject, message.Body);
            return true;
        }

        private void DeliverAsync(string destination, string subject, string body, bool htmlFormat = true)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("accounts@mediportal.com");
            mail.To.Add(destination);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = htmlFormat;

            smtpServer.Port = 587;
            smtpServer.EnableSsl = true;
            smtpServer.Credentials = new NetworkCredential("accounts@mediportal.com","MEDI@portal89");
            smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtpServer.Send(mail);
            Task.FromResult(0);
        }
    }
}