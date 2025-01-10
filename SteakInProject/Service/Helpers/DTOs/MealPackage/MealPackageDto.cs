using System;
using Domain.Common;

namespace Service.Helpers.DTOs.MealPackage
{
	public class MealPackageDto:BaseEntity
	{
        public string Title { get; set; }
        public string Desc { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal Price { get; set; }
        public List<string> ProductNames { get; set; }
    }
}

