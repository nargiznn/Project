using System;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.Testimonial
{
	public class TestimonialEditDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Text { get; set; }
        public int? Rating { get; set; }
        public IFormFile? file { get; set; }
        public bool IsPermit { get; set; }
    }
}

