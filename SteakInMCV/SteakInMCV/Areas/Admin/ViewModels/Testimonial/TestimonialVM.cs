using System;
namespace SteakInMCV.Areas.Admin.ViewModels.Testimonial
{
	public class TestimonialVM
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Text { get; set; }
        public byte Raiting { get; set; }
        public string Image { get; set; }
        public string ReviewTypeName { get; set; }
        public bool IsPermit { get; set; }
    }
}

