using System;
using SteakInMCV.Models.Enum;

namespace SteakInMCV.Areas.Admin.ViewModels.Reservation
{
	public class ReservationViewModel
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int PeopleCount { get; set; }
        public ReservationStatus Status { get; set; }
    }
}

