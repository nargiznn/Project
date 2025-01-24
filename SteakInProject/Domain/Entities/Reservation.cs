using System;
using Domain.Common;
using Domain.Enum;

namespace Domain.Entities
{
	public class Reservation:BaseEntity
	{
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int PeopleCount { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    }
}

