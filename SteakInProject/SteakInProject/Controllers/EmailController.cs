using System;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid data provided.",
                    errors = ModelState
                });
            }

            try
            {
                // E-poçt başlığı və məzmunu
                string subject = $"Yeni Mesaj - {request.FirstName} {request.LastName}";
                string body = $@"
                    <p><strong>Ad Soyad:</strong> {request.FirstName} {request.LastName}</p>
                    <p><strong>Email:</strong> {request.Email}</p>
                    <p><strong>Telefon:</strong> {request.Phone}</p>
                    <p><strong>Mesaj:</strong> {request.Message}</p>
                ";

                // E-poçt göndərilməsi
                await _emailService.SendEmailAsync("recipient-email@example.com", subject, body);

                return Ok(new { message = "Email sent successfully." });
            }
            catch (Exception ex)
            {
                // Daha ətraflı xəta qaytarılması
                return StatusCode(500, new
                {
                    message = "An error occurred while sending the email.",
                    error = ex.Message
                });
            }
        }
    }
}
