using System;
using Domain.Entities;
using Service.Helpers.DTOs.Product;

namespace Service.Services.Interfaces
{
    public interface IProductService
    {
        Task<string> CreateAsync(ProductCreateDto product);
        Task<string> EditAsync(int id, ProductEditDto product);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<string> DeleteAsync(int id);
        Task<ProductDto> GetByIdAsync(int id);
    }
}

