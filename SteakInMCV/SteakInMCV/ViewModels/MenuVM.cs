using System;
using SteakInMCV.Models;

namespace SteakInMCV.ViewModels
{
	public class MenuVM
	{
        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
        public IEnumerable<MenuCategory> MenuCategories { get; set; } = new List<MenuCategory>();
    }
}

