using System.ComponentModel.DataAnnotations;
using SteakInMCV.Models;

namespace SteakInMCV.ViewModels
{
    public class BookATableVM
    {
        [Required(ErrorMessage = "Adam sayı seçilməlidir")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Tarix seçilməlidir")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Vaxt seçilməlidir")]
        public string Time { get; set; } 

        [Required]
        public ContactFormModel ContactFormModel { get; set; }
        public IEnumerable<RestaurantTable> RestaurantTables { get; set; }
        public IEnumerable<Testimonial> Testimonials { get; set; }
        public Dictionary<string, string> Settings { get; set; }
        public List<Banner> Banners { get; set; }

    }
}
