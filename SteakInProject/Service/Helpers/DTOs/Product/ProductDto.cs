using System;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.Product
{
	public class ProductDto:BaseEntity
	{
        public string Name { get; set; }
        public string Ingredient { get; set; }
        public double Price { get; set; }
        public int SalesCount { get; set; }
        public string MenuCategoryName { get; set; }
        public string? SpecialCategoryName { get; set; }
        public string FoodCategoryName { get; set; }
        public string ProductCuisineName { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();

    }
}

