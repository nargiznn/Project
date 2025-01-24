using System;
using Domain.Entities;

namespace Service.Helpers.DTOs.MealPackage
{
	public class MealPackageCreateDto
	{
        public string Title { get; set; }
        public string Desc { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal Price { get; set; }
    }
}

