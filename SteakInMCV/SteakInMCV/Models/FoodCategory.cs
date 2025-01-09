using System;
namespace SteakInMCV.Models
{
	public class FoodCategory:BaseEntity
	{
        public string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}

