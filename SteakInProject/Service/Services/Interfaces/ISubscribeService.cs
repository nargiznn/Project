using System;
using Service.Helpers.DTOs.Subscribe;

namespace Service.Services.Interfaces
{
	public interface ISubscribeService
	{
        Task AddSubscribeAsync(SubscribeCreateDto subscribeCreateDto);
        Task<IEnumerable<SubscribeDto>> GetAllAsync();
    }
}

