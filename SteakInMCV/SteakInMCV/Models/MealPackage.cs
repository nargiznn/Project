using System;
namespace SteakInMCV.Models
{
	public class MealPackage
	{
        public string Title { get; set; }
        public string Desc { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal Price { get; set; }
        public List<string> ProductNames { get; set; }
    }
}

