
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SteakInMCV.Models;

namespace SteakInMCV.Areas.Admin.ViewModels.Testimonial
{
	public class TestimonialCreateVM
	{
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Text { get; set; }
        public byte Raiting { get; set; }
        [Required(ErrorMessage = "Rəy növünü seçmək mütləqdir")]
        public int ReviewType { get; set; }


        public List<SelectListItem> ReviewTypeList { get; set; }

        public IFormFile file { get; set; }
    }
}

