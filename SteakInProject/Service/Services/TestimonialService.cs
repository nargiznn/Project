using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.Testimonial;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class TestimonialService : ITestimonialService
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public TestimonialService(AppDbContext context,
                             IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<string> CreateAsync(TestimonialCreateDto testimonialDto)
        {

            string imagePath = null;
            if (testimonialDto.file != null)
            {
                var fileResponse = await _fileService.UploadAsync(testimonialDto.file);

                if (fileResponse.HasError)
                {
                    throw new Exception("Fotoşəkil yükləmə xətası");
                }

 
                imagePath = $"http://localhost:7031/uploads/{fileResponse.Response}";
            }
            if (testimonialDto.Raiting < 1 || testimonialDto.Raiting > 5)
            {
                throw new ArgumentOutOfRangeException("Raitinq 1-5 arasında olmalıdır.");
            }
            if (!Enum.IsDefined(typeof(ReviewType), testimonialDto.ReviewType))
            {
                throw new ArgumentException("Düzgün ReviewType daxil edilməyib.");
            }

            var testimonial = new Testimonial
            {
                Name = testimonialDto.Name?.Trim(),
                SurName = testimonialDto.Surname?.Trim(),
                Text = testimonialDto.Text?.Trim(),
                Raiting = (byte)testimonialDto.Raiting,
                Image = imagePath,
                IsPermit = false, 
                ReviewType = (ReviewType?)testimonialDto.ReviewType 
            };
            await _context.Testimonials.AddAsync(testimonial);
            await _context.SaveChangesAsync();

            return "Testimonial uğurla yaradıldı"; 
        }
        public async Task<IEnumerable<TestimonialDto>> SearchAsync(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return new List<TestimonialDto>();
            }

            var testimonials = await _context.Testimonials
                .ToListAsync(); 
            var result = testimonials
                .Where(x => (x.Name.ToLower().Trim().Contains(keyword.ToLower().Trim()) || 
                             x.SurName.ToLower().Trim().Contains(keyword.ToLower().Trim()) || 
                             x.Text.ToLower().Trim().Contains(keyword.ToLower().Trim()) || 
                             x.Raiting.ToString().Contains(keyword.Trim()) || 
                             x.IsPermit.ToString().ToLower().Contains(keyword.ToLower().Trim()) 
                ))
                .ToList();
            var testimonialDtos = result.Select(x => new TestimonialDto
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.SurName,
                Text = x.Text,
                Raiting = x.Raiting,
                Image = x.Image,
                IsPermit = x.IsPermit
            }).ToList();

            return testimonialDtos;
        }

        public async Task<string> DeleteAsync(int id)
        {
            var findData = await _context.Testimonials.FindAsync(id);

            if (findData == null)
            {
                return "Data not found";
            }
            await _fileService.DeletePath(findData.Image);
            _context.Testimonials.Remove(findData);
            await _context.SaveChangesAsync();

            return "Success";
        }

        public async Task<string> EditAsync(int id, TestimonialEditDto customer)
        {
            var findTestimonial = await _context.Testimonials.FindAsync(id);

            if (findTestimonial == null)
            {
                return "Data not found";
            }
            if (!string.IsNullOrWhiteSpace(customer.Name))
            {
                findTestimonial.Name = customer.Name.Trim();
            }
            if (!string.IsNullOrWhiteSpace(customer.Surname))
            {
                findTestimonial.SurName = customer.Surname.Trim();
            }
            if (!string.IsNullOrWhiteSpace(customer.Text))
            {
                findTestimonial.Text = customer.Text.Trim();
            }
            if (customer.Raiting.HasValue)
            {
                if (customer.Raiting < 1 || customer.Raiting > 5)
                {
                    return "Raitinq 1-5 arasında olmalıdır.";
                }
                findTestimonial.Raiting = (byte)customer.Raiting.Value;
            }
            if (customer.file != null)
            {
                if (!string.IsNullOrWhiteSpace(findTestimonial.Image))
                {
                    await _fileService.DeletePath(findTestimonial.Image);
                }
                var fileResponse = await _fileService.UploadAsync(customer.file);

                if (fileResponse.HasError)
                {
                    return fileResponse.Response;
                }

                findTestimonial.Image = $"http://localhost:7031/uploads/{fileResponse.Response}";
            }
            if (customer.ReviewType.HasValue)
            {
                findTestimonial.ReviewType = (ReviewType?)customer.ReviewType;
            }
            if (customer.IsPermit.HasValue)
            {
                findTestimonial.IsPermit = customer.IsPermit.Value;
            }
            await _context.SaveChangesAsync();

            return "Success";
        }

        public async Task<IEnumerable<TestimonialDto>> GetAllAsync()  
        {
            var testimonials = await _context.Testimonials.ToListAsync();

            var testimonialDtos = testimonials.Select(x => new TestimonialDto
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.SurName,
                Text = x.Text,
                Raiting = x.Raiting,
                Image = x.Image,
                IsPermit = x.IsPermit,
                ReviewTypeName = x.ReviewType.HasValue ? Enum.GetName(typeof(ReviewType), x.ReviewType) : string.Empty 
            }).ToList();

            return testimonialDtos;
        }


        public async Task<TestimonialDto> GetById(int id)
        {
            var testimonial = await _context.Testimonials.FirstOrDefaultAsync(x => x.Id == id);

            if (testimonial == null)
            {
                throw new KeyNotFoundException("Testimonial not found");
            }

            var testimonialDto = new TestimonialDto
            {
                Id = testimonial.Id,
                Name = testimonial.Name,
                Surname = testimonial.SurName,
                Text = testimonial.Text,
                Raiting = testimonial.Raiting,
                Image = testimonial.Image,
                IsPermit = testimonial.IsPermit,
                ReviewTypeName = testimonial.ReviewType.HasValue ? Enum.GetName(typeof(ReviewType), testimonial.ReviewType) : string.Empty 
            };

            return testimonialDto;
        }

    }
}

