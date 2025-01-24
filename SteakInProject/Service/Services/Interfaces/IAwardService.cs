using System;
using Service.Helpers.DTOs.Award;

namespace Service.Services.Interfaces
{
	public interface IAwardService
	{
        Task CreateAsync(AwardCreateDto award);
        Task<AwardDto> GetByIdAsync(int id);
        Task<IEnumerable<AwardDto>> SearchAsync(string str);
        Task<IEnumerable<AwardDto>> GetAllAsync();
        Task DeleteAsync(int id);
        Task EditAsync(int id, AwardEditDto request);
    }
}

