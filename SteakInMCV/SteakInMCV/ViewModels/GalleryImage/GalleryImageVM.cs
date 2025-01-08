using System;
using SteakInMCV.Models;

namespace SteakInMCV.ViewModels.GalleryImage
{
	public class GalleryImageVM
	{
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string FullImageUrl { get; set; }
        public string GalleryCategoryName { get; set; }

    }
}

