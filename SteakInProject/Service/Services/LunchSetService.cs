using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.Helpers.DTOs.LunchSet;
using Service.Helpers.DTOs.MealPackage;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class LunchSetService:ILunchSetService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LunchSetService(AppDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LunchSetDto>> GetAllAsync()
        {
            var lunchSet = await _context.LunchSets
               .Include(mp => mp.LunchSetProducts)
               .ThenInclude(mpp => mpp.Product)
               .AsNoTracking()
               .ToListAsync();

            return lunchSet.Select(mp => new LunchSetDto
            {
                Title = mp.Title,
                Desc = mp.Desc,
                Price = mp.Price,
                ProductNames = mp.LunchSetProducts.Select(mpp => mpp.Product.Name).ToList()
            }).ToList();
        }
    }
}
