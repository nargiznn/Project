using System;
using Domain.Entities;
using Service.Helpers.DTOs.Customer;

namespace Service.Services.Interfaces
{
	public interface ICustomerService
	{
        Task<string> CreateAsync(CustomerCreateDto customer);
        Task<string> EditAsync(int id, CustomerEditDto customer);
        Task<ICollection<Customer>> GetAllAsync();
        Task<string> DeleteAsync(int id);
        Task<Customer> GetById(int id);
    }
}

