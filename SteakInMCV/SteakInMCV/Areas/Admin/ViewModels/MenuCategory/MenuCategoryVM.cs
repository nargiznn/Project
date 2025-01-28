using System;
namespace SteakInMCV.Areas.Admin.ViewModels.MenuCategory
{
	public class MenuCategoryVM
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public List<string> ProductNames { get; set; }
        //public List<ProductVM> Products { get; set; } = new List<ProductDto>();
    }
}

