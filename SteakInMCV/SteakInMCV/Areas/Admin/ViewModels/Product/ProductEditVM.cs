using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SteakInMCV.Areas.Admin.ViewModels.Product
{
	public class ProductEditVM
	{
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Ingredient { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Qiymət sıfırdan böyük olmalıdır.")]
        public decimal? Price { get; set; }
        public int? MenuCategoryId { get; set; }
        public int? SpecialCategoryId { get; set; }
        public int? ProductCuisineId { get; set; }
        public List<IFormFile>? Files { get; set; }

    }
}
