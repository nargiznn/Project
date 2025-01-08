using System;
using Domain.Common;

namespace Service.Helpers.DTOs.GalleryImage
{
	public class GalleryImageDto:BaseEntity
	{
        public string ImageUrl { get; set; }
        public string FullImageUrl { get; set; }
        public string GalleryCategoryName { get; set; }
    }
}

