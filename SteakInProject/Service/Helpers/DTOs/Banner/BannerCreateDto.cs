using System;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.Banner
{
	public class BannerCreateDto
	{
        public IFormFile File { get; set; }
        public string ImgUrl { get; set; }
        public string AltText { get; set; }
    }
}

