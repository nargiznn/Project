using System;
using Domain.Entities;

namespace Service.Helpers.DTOs.MealPackage
{
	public class MealPackageEditDto
	{
        public string? Title { get; set; }
        public string? Desc { get; set; }
        public int? NumberOfPeople { get; set; }
        public decimal? Price { get; set; }
        public ICollection<MealPackageProduct>? MealPackageProducts { get; set; }
    }
}

