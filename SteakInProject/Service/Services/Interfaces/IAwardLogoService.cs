using System;
using Service.Helpers.DTOs.AwardLogo;

namespace Service.Services.Interfaces
{
	public interface IAwardLogoService
	{
        Task<IEnumerable<AwardLogoDto>> GetAllAsync();
        Task<AwardLogoDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task CreateAsync(AwardLogoCreateDto logo);
        Task EditAsync(int id, AwardLogoEditDto logo);
    }
}

