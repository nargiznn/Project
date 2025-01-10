using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.Helpers.DTOs.Event;
using Service.Helpers.DTOs.MealPackage;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class MealPackageService:IMealPackageService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MealPackageService(AppDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MealPackageDto>> GetAllAsync()
        {
            var mealPackages = await _context.MealPackages
                .Include(mp => mp.MealPackageProducts) 
                .ThenInclude(mpp => mpp.Product)
                .AsNoTracking()
                .ToListAsync();

            return mealPackages.Select(mp => new MealPackageDto
            {
                Id = mp.Id,
                Title = mp.Title,
                Desc = mp.Desc,
                NumberOfPeople = mp.NumberOfPeople,
                Price = mp.Price,
                ProductNames = mp.MealPackageProducts.Select(mpp => mpp.Product.Name).ToList()
            }).ToList();
        }



    }
}

