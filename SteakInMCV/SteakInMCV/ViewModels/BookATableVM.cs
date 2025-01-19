using System;
using SteakInMCV.Models;

namespace SteakInMCV.ViewModels
{
	public class BookATableVM
	{
        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
        public IEnumerable<Banner> Banners { get; set; } = new List<Banner>();
        public IEnumerable<Testimonial> Testimonials { get; set; } = new List<Testimonial>();


        public IEnumerable<RestaurantTable> RestaurantTables { get; set; } = new List<RestaurantTable>();

    }
}

