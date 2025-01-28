using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.Faq;
using Service.Helpers.DTOs.FoodCategory;
using Service.Helpers.DTOs.Product;
using Service.Helpers.Faqs;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class FaqService : IFaqService
    {
        private readonly IFaqRepository _faqRepo;
        private readonly IMapper _mapper;
        public FaqService(IFaqRepository faqRepository, IMapper mapper)
        {
            _faqRepo = faqRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(FaqCreateDto faq)
        {
            var existingFaq = await _faqRepo.GetAllWithExpression(
                x => x.Question == faq.Question
            );
            if (existingFaq.Any())
            {
                throw new ArgumentException("An Faq with the same question already exists.");
            }
            var newFaq = _mapper.Map<Faq>(faq);
            if (!faq.IsActive.HasValue)
            {
                newFaq.IsActive = false;
            }

            await _faqRepo.CreateAsync(newFaq);
        }


        public async Task DeleteAsync(int id)
        {
            await _faqRepo.DeleteAsync(id);
        }

        public async Task<IEnumerable<FaqDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<FaqDto>>(await _faqRepo.GetAllAsync());
        }

        public async Task<FaqDto> GetByIdAsync(int id)
        {
            return _mapper.Map<FaqDto>(await _faqRepo.GetByIdAsync(id));
        }

        public async Task<IEnumerable<FaqDto>> SearchAsync(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                var allFaqs = await _faqRepo.GetAllAsync();
                return _mapper.Map<IEnumerable<FaqDto>>(allFaqs);
            }
            var faqs = await _faqRepo.GetAllWithExpression(c =>
                c.Answer.Contains(str) || c.Question.Contains(str)
            );

            if (!faqs.Any())
            {
                throw new NotFoundException("No Faqs found matching the search criteria.");
            }

            return _mapper.Map<IEnumerable<FaqDto>>(faqs);
        }


        public async Task EditAsync(int id, FaqEditDto faq)
        {
            var existingFaq = await _faqRepo.GetByIdAsync(id);
            if (existingFaq == null)
            {
                throw new NotFoundException("Faq not found");
            }
            var duplicateFaq = await _faqRepo.GetAllWithExpression(
                x => x.Question == (faq.Question ?? existingFaq.Question) &&
                     x.Answer == (faq.Answer ?? existingFaq.Answer) &&
                     x.Id != id
            );

            if (duplicateFaq.Any())
            {
                throw new ArgumentException("An Faq with the same question and answer already exists.");
            }
            existingFaq.Answer = string.IsNullOrWhiteSpace(faq.Answer) ? existingFaq.Answer : faq.Answer;
            existingFaq.Question = string.IsNullOrWhiteSpace(faq.Question) ? existingFaq.Question : faq.Question;

            if (faq.IsActive.HasValue)
            {
                existingFaq.IsActive = faq.IsActive.Value;
            }

            existingFaq.UpdatedAt = DateTime.UtcNow;

            await _faqRepo.EditAsync(existingFaq);
        }

    }
}

