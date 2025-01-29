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
        public TimeSpan Time { get; set; }

        [Required(ErrorMessage = "Ad daxil edilməlidir")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad daxil edilməlidir")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email daxil edilməlidir")]
        [EmailAddress(ErrorMessage = "Email formatı düzgün deyil")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon daxil edilməlidir")]
        public string Phone { get; set; }
        //public IEnumerable<RestaurantTable> RestaurantTables { get; set; }
        public IEnumerable<Testimonial>? Testimonials { get; set; }
        public Dictionary<string, string>? Settings { get; set; }
        public List<Banner>? Banners { get; set; }


    }
}
