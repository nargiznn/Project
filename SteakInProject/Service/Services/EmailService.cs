using System;
using Microsoft.Extensions.Configuration;
using Service.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Service.Helpers;
using System.Net;
using System.Net.Mail;

namespace Service.Services
{
	public class EmailService:IEmailService
	{
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpSettings = _configuration.GetSection("Smtp");
            if (smtpSettings == null ||
                string.IsNullOrEmpty(smtpSettings["Host"]) ||
                string.IsNullOrEmpty(smtpSettings["Port"]) ||
                string.IsNullOrEmpty(smtpSettings["From"]) ||
                string.IsNullOrEmpty(smtpSettings["UserName"]) ||
                string.IsNullOrEmpty(smtpSettings["Password"]))
            {
                throw new ArgumentNullException("Smtp settings are not configured properly.");
            }

            var smtpClient = new System.Net.Mail.SmtpClient
            {
                Host = smtpSettings["Host"],
                Port = int.Parse(smtpSettings["Port"]),
                EnableSsl = bool.Parse(smtpSettings["EnableSSL"]),
                Credentials = new NetworkCredential(smtpSettings["UserName"], smtpSettings["Password"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["From"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }

    }
}

