using System;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.Banner
{
	public class BannerDto
	{
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public string AltText { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public IFormFile file { get; set; }
    }
}

