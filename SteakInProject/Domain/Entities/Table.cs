using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Table:BaseEntity
	{
		public int Capacity { get; set; }
		public int TableNumber { get; set; }
		public bool IsActive { get; set; }

	}
}

