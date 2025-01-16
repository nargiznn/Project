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

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            if (smtpSettings == null)
            {
                throw new ArgumentNullException("SmtpSettings configuration is missing.");
            }

            string server = smtpSettings["Server"];
            string senderEmail = smtpSettings["SenderEmail"];
            string senderPassword = smtpSettings["SenderPassword"];
            string port = smtpSettings["Port"];

            if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(senderEmail) ||
                string.IsNullOrWhiteSpace(senderPassword) || string.IsNullOrWhiteSpace(port))
            {
                throw new ArgumentNullException("SMTP configuration values cannot be null or empty.");
            }

            using var smtpClient = new SmtpClient(server)
            {
                Port = int.Parse(port),
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }

    }
}