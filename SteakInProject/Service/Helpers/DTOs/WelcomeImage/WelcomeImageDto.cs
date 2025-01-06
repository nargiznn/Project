using System;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.WelcomeImage
{
	public class WelcomeImageDto
	{
        public bool IsMain { get; set; }
        public IFormFile File { get; set; }
    }
}

