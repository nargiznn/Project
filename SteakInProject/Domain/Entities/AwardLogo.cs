using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities
{
	public class AwardLogo:BaseEntity
	{
		public string Image { get; set; }
		public string ImgUrl { get; set; }
		public string AltText { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}

