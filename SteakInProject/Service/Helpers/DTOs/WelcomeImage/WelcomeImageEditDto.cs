using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Service.Helpers.DTOs.WelcomeImage
{
	public class WelcomeImageEditDto
    {
        public bool? IsMain { get; set; }
        public IFormFile? File { get; set; }
    }
}

