using System;
using System.ComponentModel.DataAnnotations;
using SteakInMCV.Models;
using SteakInMCV.ViewModels.Events;
using SteakInMCV.ViewModels.GalleryImage;

namespace SteakInMCV.ViewModels
{
	public class AboutVM
	{
        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
        public IEnumerable<Chef> Chefs { get; set; } = new List<Chef>();
        public IEnumerable<Cuisine> Cuisines { get; set; } = new List<Cuisine>();
        public IEnumerable<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
        public IEnumerable<EventVM> EventVMs { get; set; } = new List<EventVM>();
        public List<Award> Awards { get; set; } = new List<Award>();
        public IEnumerable<AwardLogo> AwardLogos { get; set; } = new List<AwardLogo>();
        public IEnumerable<Banner> Banners { get; set; } = new List<Banner>();
        public IEnumerable<Faq> Faqs { get; set; } = new List<Faq>();

        public IEnumerable<GalleryCategory> GalleryCategories { get; set; } = new List<GalleryCategory>();
        public IEnumerable<GalleryImageVM> GalleryImagesVM { get; set; } = new List<GalleryImageVM>();

        public ContactFormModel ContactFormModel { get; set; } = new ContactFormModel();
    }
}

