using System;
namespace SteakInMCV.Models
{
	public class LunchSet
	{
        public string Title { get; set; }
        public string Desc { get; set; }
        public decimal Price { get; set; }
        public List<string> ProductNames { get; set; }
    }
}

