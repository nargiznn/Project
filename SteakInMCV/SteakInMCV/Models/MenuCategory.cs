using System;
namespace SteakInMCV.Models
{
	public class MenuCategory:BaseEntity
	{
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

