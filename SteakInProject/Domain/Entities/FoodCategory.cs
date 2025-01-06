using System;
using Domain.Common;

namespace Domain.Entities
{
	public class FoodCategory:BaseEntity
	{
		public string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();

    }
}

