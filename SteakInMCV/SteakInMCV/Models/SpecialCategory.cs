using System;
namespace SteakInMCV.Models
{
	public class SpecialCategory:BaseEntity
	{
        public string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}

