using System;
using Service.Helpers.DTOs.Statistic;

namespace Service.Services.Interfaces
{
	public interface IStatisticService
	{
        Task<IEnumerable<StatisticDto>> GetAllAsync();
    }
}

