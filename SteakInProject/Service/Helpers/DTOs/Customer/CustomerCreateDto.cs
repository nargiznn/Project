using System;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.Customer
{
	public class CustomerCreateDto
	{
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public IFormFile file { get; set; }
    }
}

