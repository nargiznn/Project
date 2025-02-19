using System;
using Service.Helpers.DTOs.Event;


namespace Service.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventDto>> GetAllAsync();
        Task<EventDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task CreateAsync(EventCreateDto eventDto);
        Task EditAsync(int id, EventEditDto eventDto);
        Task<IEnumerable<EventDto>> SearchAsync(string keyword);
    }
}

