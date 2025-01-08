using System;
using Domain.Common;

namespace Domain.Entities
{
	public class GalleryCategory:BaseEntity
	{
        public string Name { get; set; }
        public ICollection<GalleryImage> GalleryImages { get; set; }

    }
}

