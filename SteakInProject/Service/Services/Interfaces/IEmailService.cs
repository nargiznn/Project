using System;
namespace Service.Services.Interfaces
{
	public interface IEmailService
	{
        Task SendEmailAsync(string emailTo, string subject, string body);
        void Send(string to, string subject, string html, string from = null);
    }
}

