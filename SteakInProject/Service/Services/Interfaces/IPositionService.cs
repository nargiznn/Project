using System;
using Domain.Entities;
using Service.Helpers.DTOs.Position;

namespace Service.Services.Interfaces
{
	public interface IPositionService
	{
        Task<IEnumerable<PositionDto>> GetAllAsync();
        Task<string> CreateAsync(PositionCreateDto position);
        Task<string> EditAsync(int id, PositionEditDto position);
        Task<string> DeleteAsync(int id);
        Task<Position> GetById(int id);
    }
}

