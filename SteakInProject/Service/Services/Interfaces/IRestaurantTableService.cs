using System;
using Service.Helpers.DTOs.Table;

namespace Service.Services.Interfaces
{
	public interface IRestaurantTableService
    {
        Task<IEnumerable<RestaurantTableDto>> GetAllAsync();
    }
}

