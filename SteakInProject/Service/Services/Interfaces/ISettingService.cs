using System;
using Service.Helpers.DTOs.Setting;

namespace Service.Services.Interfaces
{
	public interface ISettingService
	{
        Task<IEnumerable<SettingDto>> GetAllAsync();
        Task<SettingDto> GetByIdAsync(int id);
        Task EditAsync(int id, SettingEditDto setting);
    }
}

