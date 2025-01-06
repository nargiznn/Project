using System;
namespace SteakInMCV.Areas.Admin.ViewModels.Slider
{
	public class SliderCreateVM
	{
        public string Title { get; set; }
        public string MainTitle { get; set; }
        public string Desc { get; set; }
        public string BtnText { get; set; }
        public IFormFile Photo { get; set; }
        public string PhotoPath { get; set; }
    }
}

