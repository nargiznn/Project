using System;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.Logo
{
	public class LogoEditDto
	{
        public string ImgUrl { get; set; }
        public string AltText { get; set; }
        public IFormFile file { get; set; }
    }
}

