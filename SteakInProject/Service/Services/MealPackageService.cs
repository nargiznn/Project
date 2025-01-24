using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Repository.Repositories;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.Event;
using Service.Helpers.DTOs.MealPackage;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class MealPackageService:IMealPackageService
	{
        private readonly IMealPackageRepository _mealPackageRepo;
        private readonly IMapper _mapper;
        public MealPackageService(IMealPackageRepository mealPackageRepository, IMapper mapper)
        {
            _mealPackageRepo = mealPackageRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(MealPackageCreateDto mealPackage)
        {
            var existingMealPackage = await _mealPackageRepo.GetAllWithExpression(
                x => x.Title == mealPackage.Title
            );
            if (existingMealPackage.Any())
            {
                throw new ArgumentException("An mealPackage with the same title or desc already exists.");
            }

            await _mealPackageRepo.CreateAsync(_mapper.Map<MealPackage>(mealPackage));
        }

        public async Task DeleteAsync(int id)
        {
            await _mealPackageRepo.DeleteAsync(id);
        }

        public async Task<IEnumerable<MealPackageDto>> GetAllAsync()
        {
            var mealPackages = await _mealPackageRepo.GetAllWithIncludeAsync(query =>
                query.Include(l => l.MealPackageProducts)
                     .ThenInclude(lp => lp.Product));

            return mealPackages.Select(ls => new MealPackageDto
            {
                Id = ls.Id,
                Title = ls.Title,
                Desc = ls.Desc,
                NumberOfPeople=ls.NumberOfPeople,
                Price = ls.Price,
                ProductNames = ls.MealPackageProducts.Select(lsp => lsp.Product.Name).ToList()
            }).ToList();
        }

        public async Task<MealPackageDto> GetByIdAsync(int id)
        {
            return _mapper.Map<MealPackageDto>(await _mealPackageRepo.GetByIdAsync(id));
        }

        public async Task<IEnumerable<MealPackageDto>> SearchAsync(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                var allMealPackages = await _mealPackageRepo.GetAllAsync();
                return _mapper.Map<IEnumerable<MealPackageDto>>(allMealPackages);
            }
            var mealPackages = await _mealPackageRepo.GetAllWithExpression(c =>
                c.Title.Contains(str) || c.Desc.Contains(str) 
            );

            if (!mealPackages.Any())
            {
                throw new NotFoundException("No mealPackage found matching the search criteria.");
            }

            return _mapper.Map<IEnumerable<MealPackageDto>>(mealPackages);
        }


        public async Task EditAsync(int id, MealPackageEditDto mealPackage)
        {
            var existingMealPackage = await _mealPackageRepo.GetByIdAsync(id);
            if (existingMealPackage == null)
            {
                throw new NotFoundException("MealPackage not found");
            }

            var duplicateMealPackage = await _mealPackageRepo.GetAllWithExpression(
                x => x.Title == (string.IsNullOrWhiteSpace(mealPackage.Title) ? existingMealPackage.Title : mealPackage.Title) &&
                     x.Desc == (string.IsNullOrWhiteSpace(mealPackage.Desc) ? existingMealPackage.Desc : mealPackage.Desc) &&
                     x.NumberOfPeople == (mealPackage.NumberOfPeople.HasValue ? mealPackage.NumberOfPeople.Value : existingMealPackage.NumberOfPeople) &&
                     x.Id != id
            );

            if (duplicateMealPackage.Any())
            {
                throw new ArgumentException("A MealPackage with the same title, description, or number of people already exists.");
            }

            existingMealPackage.Title = string.IsNullOrWhiteSpace(mealPackage.Title) ? existingMealPackage.Title : mealPackage.Title;
            existingMealPackage.Desc = string.IsNullOrWhiteSpace(mealPackage.Desc) ? existingMealPackage.Desc : mealPackage.Desc;
            existingMealPackage.NumberOfPeople = mealPackage.NumberOfPeople.HasValue ? mealPackage.NumberOfPeople.Value : existingMealPackage.NumberOfPeople;
            existingMealPackage.Price = mealPackage.Price.HasValue ? mealPackage.Price.Value : existingMealPackage.Price;

            if (mealPackage.MealPackageProducts != null)
            {
                existingMealPackage.MealPackageProducts = mealPackage.MealPackageProducts;
            }

            await _mealPackageRepo.EditAsync(existingMealPackage);
        }




    }
}

