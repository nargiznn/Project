using System;
using Domain.Common;
using Domain.Enum;

namespace Domain.Entities
{
	public class Reservation:BaseEntity
	{
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int RestaurantTableId { get; set; }
        public RestaurantTable RestaurantTable { get; set; }

        public DateTime ReservationDate { get; set; }
        public Status Status { get; set; }
    }
}

