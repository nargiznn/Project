using System;
using Service.Helpers.Faqs;

namespace Service.Services.Interfaces
{
	public interface IFaqService
	{
        Task<IEnumerable<FaqDto>> GetAllAsync();
    }
}

