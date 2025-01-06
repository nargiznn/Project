using System;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.Slider
{
	public class SliderDto
	{
        public string Title { get; set; }
        public string MainTitle { get; set; }
        public string Desc { get; set; }
        public string BtnText { get; set; }
        public IFormFile file { get; set; }
    }
}

