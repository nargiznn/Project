using System;
using Service.Helpers.DTOs.Product;
using Service.Helpers.Faqs;

namespace Service.Services.Interfaces
{
    public interface IFaqService
    {
        Task<IEnumerable<FaqDto>> GetAllAsync();
        Task<IEnumerable<FaqDto>> SearchAsync(string str);
    }
}

