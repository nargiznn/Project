using System;
using Service.Helpers.DTOs.Client;

namespace Service.Services.Interfaces
{
	public interface IClientService
	{
        Task<IEnumerable<ClientDto>> GetAllAsync();
    }
}

