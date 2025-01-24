using System;
using Service.Helpers.DTOs.MealPackage;


namespace Service.Services.Interfaces
{
	public interface IMealPackageService
	{
        Task<IEnumerable<MealPackageDto>> GetAllAsync();
        Task CreateAsync(MealPackageCreateDto mealPackage);
        Task<MealPackageDto> GetByIdAsync(int id);
        Task<IEnumerable<MealPackageDto>> SearchAsync(string str);
        Task DeleteAsync(int id);
        Task EditAsync(int id, MealPackageEditDto request);
    }
}

