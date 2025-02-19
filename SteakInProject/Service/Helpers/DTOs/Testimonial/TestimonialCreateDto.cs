using System;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.Testimonial
{
	public class TestimonialCreateDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Text { get; set; }
        public byte Raiting { get; set; }
        public ReviewType ReviewType { get; set; }
        public IFormFile file { get; set; }
    }
}

