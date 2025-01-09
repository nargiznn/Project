using System;
using SteakInMCV.Models;

namespace SteakInMCV.ViewModels
{
	public class ShopVM
	{
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<FoodCategory> FoodCategories { get; set; } = new List<FoodCategory>();
        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
    }
}

