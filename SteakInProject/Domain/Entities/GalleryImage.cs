using System;
using Domain.Common;

namespace Domain.Entities
{
    public class GalleryImage : BaseEntity
    {
        public string ImageUrl { get; set; }
        public string FullImageUrl { get; set; }
        public int GalleryCategoryId { get; set; }
        public GalleryCategory GalleryCategory { get; set; }
    }
}

