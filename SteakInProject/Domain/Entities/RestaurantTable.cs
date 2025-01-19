using System;
using Domain.Common;

namespace Domain.Entities
{
	public class RestaurantTable:BaseEntity
	{
		public int Capacity { get; set; }
		public int TableNumber { get; set; }
		public bool IsActive { get; set; }
        public ICollection<Reservation> Reservations { get; set; }

    }
}

