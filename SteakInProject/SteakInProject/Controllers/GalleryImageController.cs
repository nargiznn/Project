using System;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
	public class GalleryImageController:BaseController
	{
        private readonly IGalleryImageService _galleryimgService;

        public GalleryImageController(IGalleryImageService galleryImgService)
        {
            _galleryimgService = galleryImgService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var galleryImages = await _galleryimgService.GetAllAsync();

            foreach (var item in galleryImages)
            {
                Console.WriteLine($"ImageUrl: {item.ImageUrl}, GalleryCategoryName: {item.GalleryCategoryName}");
            }

            return Ok(galleryImages);
        }

    }
}

