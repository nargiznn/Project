using System;
using Domain.Entities;
using Service.Helpers.DTOs.Testimonial;

namespace Service.Services.Interfaces
{
	public interface ITestimonialService
    {
        Task<string> CreateAsync(TestimonialCreateDto customer);
        Task<string> EditAsync(int id, TestimonialEditDto customer);
        Task<ICollection<Testimonial>> GetAllAsync();
        Task<string> DeleteAsync(int id);
        Task<Testimonial> GetById(int id);
    }
}

