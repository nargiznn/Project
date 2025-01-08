using System;
namespace SteakInMCV.Models
{
	public class GalleryCategory:BaseEntity
	{
        public string Name { get; set; }
        public ICollection<GalleryImage> GalleryImages { get; set; }
    }
}

