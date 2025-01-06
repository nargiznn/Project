using System;
using Domain.Entities;
using Service.Helpers.DTOs.WelcomeImage;

namespace Service.Services.Interfaces
{
	public interface IWelcomeImageService
	{
        Task<string> CreateAsync(WelcomeImageCreateDto welcomeImage);
        Task<string> EditAsync(int id, WelcomeImageEditDto welcomeImage);
        Task<ICollection<WelcomeImage>> GetAllAsync();
        Task<string> DeleteAsync(int id);
        Task<WelcomeImage> GetById(int id);
    }
}

