using System;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.AwardLogo
{
	public class AwardLogoCreateDto
	{
        public IFormFile Image { get; set; }
        public string ImgUrl { get; set; }
        public string AltText { get; set; }
    }
}

