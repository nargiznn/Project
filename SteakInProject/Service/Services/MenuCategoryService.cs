using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.MealPackage;
using Service.Helpers.DTOs.MenuCategory;
using Service.Helpers.DTOs.Product;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class MenuCategoryService:IMenuCategoryService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MenuCategoryService(AppDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(MenuCategoryCreateDto menuCategory)
        {
            await _context.MenuCategories.AddAsync(_mapper.Map<MenuCategory>(menuCategory));
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var menuCategory = await _context.MenuCategories.FindAsync(id) ?? throw new NotFoundException("Data notfound");
            _context.MenuCategories.Remove(menuCategory);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, MenuCategoryEditDto menuCategory)
        {
            var existMenuCategory = await _context.MenuCategories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id) ?? throw new NotFoundException("Data notfound");

            _mapper.Map(menuCategory, existMenuCategory);

            _context.MenuCategories.Update(existMenuCategory);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MenuCategoryDto>> GetAllAsync()
        {
            var categories = await _context.MenuCategories
                                            .Include(mc => mc.Products)
                                                .ThenInclude(p => p.SpecialCategory)  // SpecialCategory'yi daxil et
                                            .Include(mc => mc.Products)
                                                .ThenInclude(p => p.FoodCategory)  // FoodCategory'yi daxil et
                                            .Include(mc => mc.Products)
                                                .ThenInclude(p => p.Cuisine)  // Cuisine'yi daxil et
                                            .Include(mc => mc.Products)
                                                .ThenInclude(p => p.ProductImages)  // ProductImages'ləri daxil et
                                            .AsNoTracking()
                                            .ToListAsync();

            var result = categories.Select(mc => new MenuCategoryDto
            {
                Id = mc.Id,
                Name = mc.Name,
                IsActive = mc.IsActive,
                Products = mc.Products.Select(p => new ProductDto
                {
                    Name = p.Name,
                    Ingredient = p.Ingredient,
                    Price = p.Price,
                    SalesCount = p.SalesCount,
                    MenuCategoryName = p.MenuCategory.Name,
                    SpecialCategoryName = p.SpecialCategory?.Name, // Null kontrolü
                    FoodCategoryName = p.FoodCategory?.Name, // Null kontrolü
                    ProductCuisineName = p.Cuisine?.Name, // Null kontrolü
                    ImageUrls = p.ProductImages.Select(pi => pi.Path).ToList()
                }).ToList()
            });

            return result;
        }



        public async Task<MenuCategoryDto> GetByIdAsync(int id)
        {
            var result = await _context.MenuCategories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (result is null) return null;

            return _mapper.Map<MenuCategoryDto>(result);
        }
    }
}

