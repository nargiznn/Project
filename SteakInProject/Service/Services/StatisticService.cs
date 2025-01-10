using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.Setting;
using Service.Helpers.DTOs.Statistic;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class StatisticService:IStatisticService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public StatisticService(AppDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IEnumerable<StatisticDto>> GetAllAsync()
        {
            var statistics = new List<StatisticDto>
            {
                new StatisticDto
                {
                    Title = "Clients Served",
                    Value = await _context.Customers.CountAsync() 
                },
                new StatisticDto
                {
                    Title = "Dishes in Menu",
                    Value = await _context.Products.CountAsync() 
                },
                new StatisticDto
                {
                    Title = "Working Hands",
                    Value = await _context.Chefs.CountAsync()
                },
                new StatisticDto
                {
                    Title = "Positive Reviews",
                    Value = await _context.Customers.CountAsync() 
                }
            };

            return statistics;
        }
    }
}

