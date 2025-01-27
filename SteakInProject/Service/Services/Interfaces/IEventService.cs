using System;
using Service.Helpers.DTOs.Event;
using Service.Helpers.DTOs.Logo;

namespace Service.Services.Interfaces
{
    public interface IEventService
    {
        Task CreateAsync(EventCreateDto events);
        Task<IEnumerable<EventDto>> GetAllAsync();
        Task<EventDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task EditAsync(int id, EventEditDto events);
    }
}

