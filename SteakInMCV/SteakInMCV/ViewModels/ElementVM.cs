using System;
using SteakInMCV.Models;

namespace SteakInMCV.ViewModels
{
	public class ElementVM
	{
        public IEnumerable<Client> Clients { get; set; } = new List<Client>();
        public IEnumerable<LunchSet> LunchSets { get; set; } = new List<LunchSet>();
        public IEnumerable<MealPackage> MealPackages { get; set; } = new List<MealPackage>();
        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, int> Statistics { get; set; } = new Dictionary<string, int>();
    }
}

