using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Customer:BaseEntity
	{
		public string Name { get; set; }
        public string Surname { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
        //public ICollection<Reservation> Reservations { get; set; }
    }
}

