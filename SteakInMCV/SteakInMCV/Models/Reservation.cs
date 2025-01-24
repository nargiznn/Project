using System;
using System.ComponentModel.DataAnnotations;
using SteakInMCV.Models.Enum;

namespace SteakInMCV.Models
{
	public class Reservation
    {
        //public string Name { get; set; }
        //public string Surname { get; set; }
        //public string Email { get; set; }
        //public string PhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int PeopleCount { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        public ContactFormModel ContactForm { get; set; }
    }
}

