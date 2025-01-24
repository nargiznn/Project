using System;
using Service.Helpers.DTOs.Award;
using Service.Helpers.DTOs.SpecialCategory;
using Service.Helpers.DTOs.Tag;

namespace Service.Services.Interfaces
{
	public interface ISpecialCategoryService
	{
        Task CreateAsync(SpecialCategoryCreateDto specialCategory);
        Task<IEnumerable<SpecialCategoryDto>> GetAllAsync();
        Task<SpecialCategoryDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task EditAsync(int id, SpecialCategoryEditDto specialCategory);
        Task<IEnumerable<SpecialCategoryDto>> SearchAsync(string str);

    }
}

