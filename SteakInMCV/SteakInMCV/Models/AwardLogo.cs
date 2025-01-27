using System;
namespace SteakInMCV.Models
{
	public class AwardLogo:BaseEntity
	{
        public int Id { get; set; }
        public string Image { get; set; }
        public string ImgUrl { get; set; }
        public string AltText { get; set; }
    }
}

