using System;
namespace Service.Helpers.DTOs.Reservation
{
	public class ReservationResponseDto
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int PeopleCount { get; set; }
        public string Status { get; set; }
    }
}

