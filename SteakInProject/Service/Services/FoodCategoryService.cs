using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.FoodCategory;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class FoodCategoryService:IFoodCategoryService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FoodCategoryService(AppDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(FoodCategoryCreateDto foodCategory)
        {
            await _context.FoodCategories.AddAsync(_mapper.Map<FoodCategory>(foodCategory));
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var foodCategory = await _context.FoodCategories.FindAsync(id) ?? throw new NotFoundException("Data notfound");
            _context.FoodCategories.Remove(foodCategory);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, FoodCategoryEditDto foodCategory)
        {
            var existFoodCategory = await _context.FoodCategories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id) ?? throw new NotFoundException("Data notfound");

            _mapper.Map(foodCategory, existFoodCategory);

            _context.FoodCategories.Update(existFoodCategory);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FoodCategoryDto>> GetAllAsync()
        {
            return _mapper.Map<List<FoodCategoryDto>>(await _context.FoodCategories.AsNoTracking().ToListAsync());
        }

        public async Task<FoodCategoryDto> GetByIdAsync(int id)
        {
            var result = await _context.FoodCategories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (result is null) return null;

            return _mapper.Map<FoodCategoryDto>(result);
        }
    }
}

