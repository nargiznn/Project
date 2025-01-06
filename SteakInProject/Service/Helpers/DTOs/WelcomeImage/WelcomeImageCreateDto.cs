using System;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.WelcomeImage
{
	public class WelcomeImageCreateDto
	{
        public IFormFile File { get; set; }
    }
}

