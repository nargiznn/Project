using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service.Helpers.DTOs.AwardLogo
{
	public class AwardLogoDto
	{
        public int Id { get; set; }
        public string Image { get; set; }
        public string ImgUrl { get; set; }
        public string AltText { get; set; }
    }
}

