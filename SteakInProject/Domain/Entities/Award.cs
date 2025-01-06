using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Award:BaseEntity
	{
		public string Name { get; set; }
		public DateTime Year { get; set; }
	}
}

