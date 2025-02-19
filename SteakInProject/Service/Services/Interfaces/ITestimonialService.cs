using System;
using Domain.Entities;
using Service.Helpers.DTOs.Testimonial;

namespace Service.Services.Interfaces
{
	public interface ITestimonialService
    {
        Task<string> CreateAsync(TestimonialCreateDto testimonialDto);
        Task<string> EditAsync(int id, TestimonialEditDto customer);
        Task<IEnumerable<TestimonialDto>> GetAllAsync(); 
        Task<string> DeleteAsync(int id);
        Task<TestimonialDto> GetById(int id); 
        Task<IEnumerable<TestimonialDto>> SearchAsync(string keyword);
    }
}

