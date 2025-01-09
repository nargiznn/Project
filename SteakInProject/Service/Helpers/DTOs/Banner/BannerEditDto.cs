using System;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.Banner
{
	public class BannerEditDto
	{
        public IFormFile? file { get; set; }
        public string ImgUrl { get; set; }
        public string AltText { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
    }
}

