using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.GalleryCategory;
using Service.Helpers.DTOs.MenuCategory;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class GalleryCategoryService:IGalleryCategoryService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GalleryCategoryService(AppDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GalleryCategoryDto>> GetAllAsync()
        {
            return _mapper.Map<List<GalleryCategoryDto>>(await _context.GalleryCategories.AsNoTracking().ToListAsync());
        }

    }
}

