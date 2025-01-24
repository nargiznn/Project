using System;
using Service.Helpers.DTOs.LunchSet;
using Service.Helpers.DTOs.MealPackage;

namespace Service.Services.Interfaces
{
	public interface ILunchSetService
	{
        Task CreateAsync(LunchSetCreateDto lunchSet);
        Task<LunchSetDto> GetByIdAsync(int id);
        Task<IEnumerable<LunchSetDto>> SearchAsync(string str);
        Task<IEnumerable<LunchSetDto>> GetAllAsync();
        Task DeleteAsync(int id);
        Task EditAsync(int id, LunchSetEditDto request);
    }
}

