using System;
using Service.Helpers.DTOs.MenuCategory;

namespace Service.Services.Interfaces
{
	public interface IMenuCategoryService
	{
        Task CreateAsync(MenuCategoryCreateDto menuCategory);
        Task<IEnumerable<MenuCategoryDto>> GetAllAsync();
        Task<MenuCategoryDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task EditAsync(int id, MenuCategoryEditDto menuCategory);
    }
}

