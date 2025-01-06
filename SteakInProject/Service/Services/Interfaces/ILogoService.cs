using System;
using Domain.Entities;
using Service.Helpers.DTOs.Logo;

namespace Service.Services.Interfaces
{
	public interface IAwardLogoService
	{
        Task<string> CreateAsync(LogoCreateDto logo);
        Task<string> EditAsync(int id, LogoEditDto logo);
        Task<ICollection<AwardLogo>> GetAllAsync();
        Task<string> DeleteAsync(int id);
        Task<AwardLogo> GetById(int id);
    }
}

