using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
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
            if (string.IsNullOrEmpty(reservation.Email) || !new EmailAddressAttribute().IsValid(reservation.Email))
            {
                throw new ArgumentException("Invalid email address.");
            }
            if (!string.IsNullOrEmpty(reservation.PhoneNumber) && !Regex.IsMatch(reservation.PhoneNumber, @"^\+?[0-9]{10,15}$"))
            {
                throw new ArgumentException("Invalid phone number.");
            }
            var reservationDateTime = reservation.Date.Date.Add(reservation.Time);
            if (reservationDateTime <= DateTime.Now)
            {
                throw new ArgumentException("Reservation date and time must be in the future.");
            }
            _dbContext.Reservations.Add(reservation);
            await _dbContext.SaveChangesAsync();
            string subject = "Reservation Pending";
            string message = $"Your reservation is currently pending and will be reviewed shortly. Reservation details: <br/> Date: {reservation.Date:MMMM dd, yyyy} <br/> Time: {reservation.Time}";

            var email = reservation.Email;
            if (!string.IsNullOrEmpty(email))
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
            else
            {
                Console.WriteLine("Email address is null or empty.");
            }

            return reservation;
        }
        public async Task<List<Reservation>> GetReservationsAsync()
        {
            var reservations = await _dbContext.Reservations
                .Select(r => new Reservation
                {
                    Id = r.Id,
                    Name = r.Name ?? string.Empty,  
                    Surname = r.Surname ?? string.Empty,
                    Email = r.Email ?? string.Empty,
                    PhoneNumber = r.PhoneNumber ?? string.Empty,
                    Date = r.Date,
                    Time = r.Time,
                    PeopleCount = r.PeopleCount,
                    Status = r.Status
                })
                .ToListAsync();

            return reservations;
        }


        public async Task<Reservation> UpdateReservationStatusAsync(int id, ReservationStatus status)
        {
            var reservation = await _dbContext.Reservations.FindAsync(id);

            if (reservation == null)
            {
                return null;
            }

            reservation.Status = status;
            await _dbContext.SaveChangesAsync();

            if (!string.IsNullOrEmpty(reservation.Email))
            {
                string subject = status == ReservationStatus.Approved ? "Reservation Accepted" : "Reservation Canceled";
                string message = status == ReservationStatus.Approved
                                 ? $"Your reservation has been accepted. Reservation details: <br/> Date: {reservation.Date:MMMM dd, yyyy} <br/> Time: {reservation.Time}"
                                 : $"Your reservation has been canceled. Reservation details: <br/> Date: {reservation.Date:MMMM dd, yyyy} <br/> Time: {reservation.Time}";

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(MailboxAddress.Parse("nargizzn@code.edu.az"));
                mimeMessage.To.Add(MailboxAddress.Parse(reservation.Email));
                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart(TextFormat.Html) { Text = message };

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("nargizzn@code.edu.az", "yswa bxqt nfqf iifz");
                smtp.Send(mimeMessage);
                smtp.Disconnect(true);
            }
            else
            {
                Console.WriteLine("No valid email address found for this reservation.");
            }

            return reservation;
        }


    }
}
