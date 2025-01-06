using System;
using Service.Helpers.DTOs.Cuisine;

namespace Service.Services.Interfaces
{
	public interface ICuisineService
	{
        Task CreateAsync(CuisineCreateDto cuisine);
        Task<IEnumerable<CuisineDto>> GetAllAsync();
        Task<CuisineDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task EditAsync(int id, CuisineEditDto cuisine);
    }
}

