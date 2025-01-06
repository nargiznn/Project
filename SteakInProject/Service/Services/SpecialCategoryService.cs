using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.SpecialCategory;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class SpecialCategoryService:ISpecialCategoryService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SpecialCategoryService(AppDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(SpecialCategoryCreateDto specialCategory)
        {
            await _context.SpecialCategories.AddAsync(_mapper.Map<SpecialCategory>(specialCategory));
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var specialCategory = await _context.SpecialCategories.FindAsync(id) ?? throw new NotFoundException("Data notfound");
            _context.SpecialCategories.Remove(specialCategory);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, SpecialCategoryEditDto specialCategory)
        {
            var existSpecialCategory = await _context.SpecialCategories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id) ?? throw new NotFoundException("Data notfound");

            _mapper.Map(specialCategory, existSpecialCategory);

            _context.SpecialCategories.Update(existSpecialCategory);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SpecialCategoryDto>> GetAllAsync()
        {
            return _mapper.Map<List<SpecialCategoryDto>>(await _context.SpecialCategories.AsNoTracking().ToListAsync());
        }

        public async Task<SpecialCategoryDto> GetByIdAsync(int id)
        {
            var result = await _context.SpecialCategories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (result is null) return null;

            return _mapper.Map<SpecialCategoryDto>(result);
        }
    }
}

