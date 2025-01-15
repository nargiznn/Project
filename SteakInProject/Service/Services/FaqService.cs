using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.FoodCategory;
using Service.Helpers.DTOs.Product;
using Service.Helpers.Faqs;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class FaqService:IFaqService
	{
        private readonly IFaqRepository _faqRepo;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FaqService(AppDbContext context,
                              IMapper mapper,
                              IFaqRepository faqRepository)
        {
            _context = context;
            _mapper = mapper;
            _faqRepo = faqRepository;
        }

        public async Task<IEnumerable<FaqDto>> GetAllAsync()
        {
            return _mapper.Map<List<FaqDto>>(await _context.Faqs.AsNoTracking().ToListAsync());
        }
        public async Task<IEnumerable<FaqDto>> SearchAsync(string str)
        {
            var faqs = await _faqRepo.GetAllWithExpression(c => c.Question.Contains(str) || c.Answer.Contains(str));
            return _mapper.Map<IEnumerable<FaqDto>>(faqs);
        }

    }
}

