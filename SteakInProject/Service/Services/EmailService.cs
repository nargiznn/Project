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
        private readonly AppSettings _appSettings;

        public EmailService(IConfiguration configuration,
                            IOptions<AppSettings> appSettings)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        public async Task SendEmailAsync(string emailTo, string subject, string body)
        {
            if (string.IsNullOrEmpty(emailTo))
            {
                Console.WriteLine("Email address is null or empty.");
                return; 
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
        public void Send(string to, string subject, string html, string from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _appSettings.From));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_appSettings.Host, _appSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_appSettings.UserName, _appSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }


    }
}