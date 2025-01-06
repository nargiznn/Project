using System;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.Slider
{
	public class SliderCreateDto
	{
        public string Title { get; set; }
        public string MainTitle { get; set; }
        public string Desc { get; set; }
        public string BtnText { get; set; }
        public IFormFile File { get; set; }
    }
}

