using System;
using Domain.Entities;
using Service.Helpers.DTOs.Banner;

namespace Service.Services.Interfaces
{
	public interface IBannerService
	{
        Task<string> CreateAsync(BannerCreateDto banner);
        Task<string> EditAsync(int id, BannerEditDto banner);
        Task<ICollection<Banner>> GetAllAsync();
        Task<string> DeleteAsync(int id);
        Task<Banner> GetById(int id);
    }
}

