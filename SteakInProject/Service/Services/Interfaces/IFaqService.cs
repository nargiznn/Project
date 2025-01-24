using System;
using Service.Helpers.DTOs.Faq;
using Service.Helpers.DTOs.Product;
using Service.Helpers.Faqs;

namespace Service.Services.Interfaces
{
    public interface IFaqService
    {
        Task CreateAsync(FaqCreateDto faq);
        Task<FaqDto> GetByIdAsync(int id);
        Task<IEnumerable<FaqDto>> SearchAsync(string str);
        Task<IEnumerable<FaqDto>> GetAllAsync();
        Task DeleteAsync(int id);
        Task EditAsync(int id, FaqEditDto request);
    }
}

