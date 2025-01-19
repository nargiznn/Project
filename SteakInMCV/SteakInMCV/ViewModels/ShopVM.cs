using System;
using SteakInMCV.Models;
using SteakInMCV.ViewModels.Events;

namespace SteakInMCV.ViewModels
{
	public class ShopVM
	{
        public Product Product { get; set; }
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<FoodCategory> FoodCategories { get; set; } = new List<FoodCategory>();
        public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();
        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
    }
}

