using System;
using SteakInMCV.Models;

namespace SteakInMCV.ViewModels
{
	public class BookATableVM
	{
        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
        public IEnumerable<Banner> Banners { get; set; } = new List<Banner>();
        public IEnumerable<Customer> Customers { get; set; } = new List<Customer>();
    }
}

