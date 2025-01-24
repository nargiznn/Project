using System;
using Microsoft.Extensions.Configuration;
using Service.Services.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Service.Helpers;
using System.Net;
using System.Net.Mail;

namespace Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string emailTo, string subject, string body)
        {
            if (string.IsNullOrEmpty(emailTo))
            {
                throw new ArgumentNullException(nameof(emailTo), "Email address cannot be null or empty.");
            }

            var smtpSettings = _configuration.GetSection("SmtpSettings");

            SmtpClient smtpClient = new SmtpClient(smtpSettings["Server"], Convert.ToInt32(smtpSettings["Port"]));

            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(smtpSettings["SenderEmail"], smtpSettings["SenderPassword"]);

            MailAddress from = new MailAddress(smtpSettings["SenderEmail"], "Steak-In");
            MailAddress to = new MailAddress(emailTo);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(message);
        }


    }
}