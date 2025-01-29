using System;
using System.ComponentModel.DataAnnotations;

namespace SteakInMCV.Models
{
	public class ReservationForm
	{
        [Required(ErrorMessage = "Ad daxil edilməlidir")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad daxil edilməlidir")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email daxil edilməlidir")]
        [EmailAddress(ErrorMessage = "Email formatı düzgün deyil")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon daxil edilməlidir")]
        public string Phone { get; set; }
    }
}

