using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.GalleryImage;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class GalleryImageService : IGalleryImageService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public GalleryImageService(AppDbContext context,
                           IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GalleryImageDto>> GetAllAsync()
        {
            var galleryImages = await _context.GalleryImages
                .AsNoTracking()
                .Include(x => x.GalleryCategory) 
                .ToListAsync();

            return _mapper.Map<IEnumerable<GalleryImageDto>>(galleryImages);
        }


    }
}

