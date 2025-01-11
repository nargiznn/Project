using System;
using Service.Helpers.DTOs.Product;

namespace Service.Helpers.DTOs.MenuCategory
{
	public class MenuCategoryDto
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<string> ProductNames { get; set; }
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}

