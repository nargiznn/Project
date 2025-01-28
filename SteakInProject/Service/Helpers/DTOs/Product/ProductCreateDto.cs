using System;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.Product
{
	public class ProductCreateDto
	{
        public string Name { get; set; }
        public string Ingredient { get; set; }
        public double Price { get; set; }
        public int MenuCategoryId { get; set; }
        public int? SpecialCategoryId { get; set; }
        public int CuisineId { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}

