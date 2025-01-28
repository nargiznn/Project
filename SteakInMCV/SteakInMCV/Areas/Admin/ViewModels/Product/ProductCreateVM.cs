using System;
using System.ComponentModel.DataAnnotations;
using SteakInMCV.Areas.Admin.ViewModels.Cuisine;
using SteakInMCV.Areas.Admin.ViewModels.MenuCategory;
using SteakInMCV.Areas.Admin.ViewModels.SpecialCategory;

namespace SteakInMCV.Areas.Admin.ViewModels.Product
{
	public class ProductCreateVM
	{
		 public string Name { get; set; }
        public string Ingredient { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Qiymət sıfırdan böyük olmalıdır.")]
        public decimal Price { get; set; }
        public int MenuCategoryId { get; set; }
        public int? SpecialCategoryId { get; set; }
        public int CuisineId { get; set; }
        public List<IFormFile> Files { get; set; }

    }
}

