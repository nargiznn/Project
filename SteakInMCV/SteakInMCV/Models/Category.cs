using System;
namespace SteakInMCV.Models
{
	public class Category
	{
        public string MenuCategoryName { get; set; }
        public List<Product> Products { get; set; }
    }
}

