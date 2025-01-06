using System;
namespace SteakInMCV.Models
{
	public class Cuisine:BaseEntity
	{
        public string Name { get; set; }
        public string Desc { get; set; }
        public ICollection<Product> Products { get; set; }
        public int ProgressPercentage { get; set; }
    }
}

