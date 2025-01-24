using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enum;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using Repository.Data;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _dbContext;
        private readonly IEmailService _emailService;

        public ReservationService(AppDbContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            _dbContext.Reservations.Add(reservation);
            await _dbContext.SaveChangesAsync();

            string subject = "Reservation Pending";
            string message = "Your reservation is currently pending and will be reviewed shortly.";

            var emailAddresses = await _dbContext.Users
                                                  .Where(u => !string.IsNullOrEmpty(u.Email))
                                                  .Select(u => u.Email)
                                                  .ToListAsync();

            if (emailAddresses != null && emailAddresses.Any())
            {
                foreach (var email in emailAddresses)
                {
                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(MailboxAddress.Parse("nargizzn@code.edu.az"));
                    mimeMessage.To.Add(MailboxAddress.Parse(email));
                    mimeMessage.Subject = subject;
                    mimeMessage.Body = new TextPart(TextFormat.Html) { Text = message };

                    using var smtp = new SmtpClient();
                    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate("nargizzn@code.edu.az", "yswa bxqt nfqf iifz");
                    smtp.Send(mimeMessage);
                    smtp.Disconnect(true);
                }
            }
            else
            {
                Console.WriteLine("No active users found.");
            }

            return reservation;
        }

        public async Task<List<Reservation>> GetReservationsAsync()
        {
            return await _dbContext.Reservations.ToListAsync();
        }

        public async Task<Reservation> UpdateReservationStatusAsync(int id, ReservationStatus status)
        {
            var reservation = await _dbContext.Reservations.FindAsync(id);
            if (reservation == null) return null;

            reservation.Status = status;
            _dbContext.Reservations.Update(reservation);
            await _dbContext.SaveChangesAsync();

            string subject = status switch
            {
                ReservationStatus.Approved => "Reservation Approved",
                ReservationStatus.Rejected => "Reservation Canceled",
                _ => "Reservation Status Updated"
            };

            string message = status switch
            {
                ReservationStatus.Approved => "Your reservation has been approved.",
                ReservationStatus.Rejected => "Your reservation has been canceled.",
                _ => "Your reservation status has been updated."
            };

            var emailAddresses = await _dbContext.Users
                                                  .Where(u => !string.IsNullOrEmpty(u.Email))
                                                  .Select(u => u.Email)
                                                  .ToListAsync();

            if (emailAddresses != null && emailAddresses.Any())
            {
                foreach (var email in emailAddresses)
                {
                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(MailboxAddress.Parse("nargizzn@code.edu.az"));
                    mimeMessage.To.Add(MailboxAddress.Parse(email));
                    mimeMessage.Subject = subject;
                    mimeMessage.Body = new TextPart(TextFormat.Html) { Text = message };

                    using var smtp = new SmtpClient();
                    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate("nargizzn@code.edu.az", "yswa bxqt nfqf iifz");
                    smtp.Send(mimeMessage);
                    smtp.Disconnect(true);
                }
            }
            else
            {
                Console.WriteLine("No active users found.");
            }

            return reservation;
        }
    }
}
