using System;
using Service.Helpers.DTOs.Chef;


namespace Service.Services.Interfaces
{
	public interface IChefService
	{
        Task<IEnumerable<ChefDto>> GetAllAsync();
        Task<ChefDto> GetByIdAsync(int id);
        Task CreateAsync(ChefCreateDto chef);
        Task EditAsync(int id, ChefEditDto updatedChef);
        Task DeleteAsync(int id);
        Task AddPosition(int chefId, int positionId);
    }
}

