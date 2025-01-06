using System;
using Service.Helpers.DTOs.Award;

namespace Service.Services.Interfaces
{
	public interface IAwardService
	{
        Task CreateAsync(AwardCreateDto award);
        Task<IEnumerable<AwardDto>> GetAllAsync();
        Task<AwardDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task EditAsync(int id, AwardEditDto award);
    }
}

