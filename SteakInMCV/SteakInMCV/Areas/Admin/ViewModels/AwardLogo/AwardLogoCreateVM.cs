using System;
namespace SteakInMCV.Areas.Admin.ViewModels.AwardLogo
{
	public class AwardLogoCreateVM
    {
        public string ImgUrl { get; set; }
        public string AltText { get; set; }
        public IFormFile Image { get; set; }
    }
}

