using System;
using Microsoft.Extensions.Configuration;
using Service.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Service.Helpers;

namespace Service.Services
{
	public class EmailService:IEmailService
	{
        private readonly AppSettings _appSettings;

        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task SendAsync(string to, string subject, string html, string from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _appSettings.From));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email asynchronously
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_appSettings.Host, _appSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_appSettings.UserName, _appSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}

