using System;
using Service.Helpers.DTOs.MealPackage;


namespace Service.Services.Interfaces
{
	public interface IMealPackageService
	{
        Task<IEnumerable<MealPackageDto>> GetAllAsync();
    }
}

