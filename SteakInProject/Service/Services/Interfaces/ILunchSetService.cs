using System;
using Service.Helpers.DTOs.LunchSet;
using Service.Helpers.DTOs.MealPackage;

namespace Service.Services.Interfaces
{
	public interface ILunchSetService
	{
        Task<IEnumerable<LunchSetDto>> GetAllAsync();
    }
}

