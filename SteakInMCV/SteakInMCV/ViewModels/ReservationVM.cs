using System;
using System.ComponentModel.DataAnnotations;

namespace SteakInMCV.ViewModels
{
	public class ReservationVM
	{
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        [Required]
        public int PeopleCount { get; set; }

    }
}

