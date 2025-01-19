using System;
using Domain.Common;
using Domain.Entities;

namespace Service.Helpers.DTOs.Table
{
	public class RestaurantTableDto : BaseEntity
	{
        public int Capacity { get; set; }
        public int TableNumber { get; set; }
        public bool IsActive { get; set; }
        //public ICollection<Reservation> Reservations { get; set; }
    }
}

