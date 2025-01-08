using System;
using Service.Helpers.DTOs.GalleryImage;

namespace Service.Services.Interfaces
{
	public interface IGalleryImageService
	{
        Task<IEnumerable<GalleryImageDto>> GetAllAsync();
    }
}

