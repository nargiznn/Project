using System;
using Domain.Common;

namespace Domain.Entities
{
	public class MenuCategory:BaseEntity
	{
		public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}

