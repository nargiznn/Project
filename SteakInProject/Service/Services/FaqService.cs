using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.FoodCategory;
using Service.Helpers.Faqs;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class FaqService:IFaqService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FaqService(AppDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FaqDto>> GetAllAsync()
        {
            return _mapper.Map<List<FaqDto>>(await _context.Faqs.AsNoTracking().ToListAsync());
        }
    }
}

