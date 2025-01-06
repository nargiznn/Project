using System;
using Service.Helpers.DTOs.WelcomeInfo;

namespace Service.Services.Interfaces
{
	public interface IWelcomeInfoService
    {
        Task CreateAsync(WelcomeInfoCreateDto welcomeInfo);
        Task<WelcomeInfoDto> GetByIdAsync(int id);
        Task<IEnumerable<WelcomeInfoDto>> GetAllAsync();
        Task DeleteAsync(int id);
        Task EditAsync(int id, WelcomeInfoEditDto welcomeInfo);
    }
}


