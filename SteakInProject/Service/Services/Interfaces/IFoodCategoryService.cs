using System;
using Service.Helpers.DTOs.FoodCategory;

namespace Service.Services.Interfaces
{
	public interface IFoodCategoryService
	{
        Task CreateAsync(FoodCategoryCreateDto foodCategory);
        Task<IEnumerable<FoodCategoryDto>> GetAllAsync();
        Task<FoodCategoryDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task EditAsync(int id, FoodCategoryEditDto foodCategory);
    }
}

