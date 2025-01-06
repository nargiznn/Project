using System;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.Customer
{
	public class CustomerDto
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public IFormFile file { get; set; }
    }
}

