using System;
using Domain.Entities;
using Service.Helpers.DTOs.Slider;

namespace Service.Services.Interfaces
{
	public interface ISliderService
    {
        Task<string> CreateAsync(SliderCreateDto slider);
        Task<string> EditAsync(int id, SliderEditDto slider);
        Task<ICollection<Slider>> GetAllAsync();
        Task<string> DeleteAsync(int id);
        Task<Slider> GetById(int id);
    }
}

