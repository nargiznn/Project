using System;
namespace SteakInMCV.Models
{
	public class Banner:BaseEntity
	{
        public string Image { get; set; }
        public string ImgUrl { get; set; }
        public string AltText { get; set; }
    }
}

