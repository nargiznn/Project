using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.MealPackage;
using Service.Helpers.DTOs.MenuCategory;
using Service.Helpers.DTOs.Product;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class MenuCategoryService:IMenuCategoryService
	{
        private readonly IMenuCategoryRepository _menuCategoryRepo;
        private readonly IMapper _mapper;
        public MenuCategoryService(IMenuCategoryRepository menuCatRepository, IMapper mapper)
        {
            _menuCategoryRepo = menuCatRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(MenuCategoryCreateDto menuCategory)
        {
            var existingMenuCategory = await _menuCategoryRepo.GetAllWithExpression(
                x => x.Name == menuCategory.Name
            );
            if (existingMenuCategory.Any())
            {
                throw new ArgumentException("An MenuCategory with the same name already exists.");
            }
            var newMenuCategory = _mapper.Map<MenuCategory>(menuCategory);
            if (!menuCategory.IsActive.HasValue)
            {
                newMenuCategory.IsActive = false;
            }

            await _menuCategoryRepo.CreateAsync(newMenuCategory);
        }


        public async Task DeleteAsync(int id)
        {
            await _menuCategoryRepo.DeleteAsync(id);
        }

        public async Task<IEnumerable<MenuCategoryDto>> GetAllAsync()
        {
            var categories = await _menuCategoryRepo.GetAllWithIncludeAsync(
                include: query => query.Include(mc => mc.Products)
                                       .ThenInclude(p => p.SpecialCategory)
                                       .Include(mc => mc.Products)
                                       //.ThenInclude(p => p.FoodCategory)
                                       .Include(mc => mc.Products)
                                       .ThenInclude(p => p.Cuisine)
                                       .Include(mc => mc.Products)
                                       .ThenInclude(p => p.ProductImages)
            );

            var result = categories.Cast<MenuCategory>().Select(mc => new MenuCategoryDto
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
                    MenuCategoryName = mc.Name,
                    SpecialCategoryName = p.SpecialCategory?.Name,
                    ProductCuisineName = p.Cuisine?.Name,
                    ImageUrls = p.ProductImages.Select(pi => pi.Path).ToList()
                }).ToList()
            });

            return result;
        }



        public async Task<MenuCategoryDto> GetByIdAsync(int id)
        {
            return _mapper.Map<MenuCategoryDto>(await _menuCategoryRepo.GetByIdAsync(id));
        }

        public async Task<IEnumerable<MenuCategoryDto>> SearchAsync(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                var allMenuCategorys = await _menuCategoryRepo.GetAllAsync();
                return _mapper.Map<IEnumerable<MenuCategoryDto>>(allMenuCategorys);
            }
            var menuCategories = await _menuCategoryRepo.GetAllWithExpression(c =>
                c.Name.Contains(str) || c.Name.Contains(str)
            );

            if (!menuCategories.Any())
            {
                throw new NotFoundException("No MenuCategory found matching the search criteria.");
            }

            return _mapper.Map<IEnumerable<MenuCategoryDto>>(menuCategories);
        }


        public async Task EditAsync(int id, MenuCategoryEditDto menuCategory)
        {
            var existingMenuCategory = await _menuCategoryRepo.GetByIdAsync(id);
            if (existingMenuCategory == null)
            {
                throw new NotFoundException("MenuCategory not found");
            }
            var duplicateMenuCategory = await _menuCategoryRepo.GetAllWithExpression(
                x => x.Name == (menuCategory.Name ?? existingMenuCategory.Name) &&
                     x.Id != id
            );

            if (duplicateMenuCategory.Any())
            {
                throw new ArgumentException("An MenuCategory with the same name already exists.");
            }
            existingMenuCategory.Name = string.IsNullOrWhiteSpace(menuCategory.Name) ? existingMenuCategory.Name : menuCategory.Name;

            if (menuCategory.IsActive.HasValue)
            {
                existingMenuCategory.IsActive = menuCategory.IsActive.Value;
            }
            await _menuCategoryRepo.EditAsync(existingMenuCategory);
        }

    }
}

