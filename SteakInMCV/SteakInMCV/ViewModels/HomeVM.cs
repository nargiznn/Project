using System;
using SteakInMCV.Models;
using SteakInMCV.ViewModels.Events;

namespace SteakInMCV.ViewModels
{
	public class HomeVM
	{
        public IEnumerable<Slider> Sliders { get; set; } = new List<Slider>();
        public IEnumerable<EventVM> EventVMs { get; set; } = new List<EventVM>();
        public IEnumerable<Customer> Customers { get; set; } = new List<Customer>();
        public IEnumerable<MenuCategory> MenuCategories { get; set; } = new List<MenuCategory>();
        public IEnumerable<SpecialCategory> SpecialCategories { get; set; } = new List<SpecialCategory>();
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();
        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
    }
}

