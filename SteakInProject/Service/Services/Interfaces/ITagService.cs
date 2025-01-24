using System;
using Service.Helpers.DTOs.Award;
using Service.Helpers.DTOs.Tag;

namespace Service.Services.Interfaces
{
	public interface ITagService
	{
        Task CreateAsync(TagCreateDto tag);
        Task<IEnumerable<TagDto>> GetAllAsync();
        Task<TagDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task EditAsync(int id, TagEditDto tag);
        Task<IEnumerable<TagDto>> SearchAsync(string str);

    }
}

