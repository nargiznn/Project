using System;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.Logo
{
	public class LogoDto
	{
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public string AltText { get; set; }
        public IFormFile file { get; set; }
    }
}

