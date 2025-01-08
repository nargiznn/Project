using System;
using Service.Helpers.DTOs.GalleryCategory;

namespace Service.Services.Interfaces
{
	public interface IGalleryCategoryService
	{
        Task<IEnumerable<GalleryCategoryDto>> GetAllAsync();
    }
}

