using System.Net;
using System.Net.Mail;
using WebFormAPI.Models;
using Microsoft.Extensions.Options;

namespace WebFormAPI.SendEmail
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _mailSettings;
        public EmailService(IOptions<EmailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public void SendEmail(string toEmail, string subject, string body)
        {
            var fromAddress = new MailAddress(_mailSettings.FromMail!, _mailSettings.FromDisplayName);
            var toAddress = new MailAddress(toEmail);

            var smtp = new SmtpClient
            {
                Host = _mailSettings.Host!,
                Port = _mailSettings.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, _mailSettings.Password)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
