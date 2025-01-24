using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.LunchSet;
using Service.Helpers.DTOs.MealPackage;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class LunchSetService:ILunchSetService
	{
        private readonly ILunchSetRepository _lunchSetRepo;
        private readonly IMapper _mapper;
        public LunchSetService(ILunchSetRepository lunchSetRepository, IMapper mapper)
        {
            _lunchSetRepo = lunchSetRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(LunchSetCreateDto lunchSet)
        {
            var existingLunchSet = await _lunchSetRepo.GetAllWithExpression(
                x => x.Title == lunchSet.Title && x.Desc == lunchSet.Desc
            );
            if (existingLunchSet.Any())
            {
                throw new ArgumentException("An lunchset with the same title or desc already exists.");
            }

            await _lunchSetRepo.CreateAsync(_mapper.Map<LunchSet>(lunchSet));
        }

        public async Task DeleteAsync(int id)
        {
            await _lunchSetRepo.DeleteAsync(id);
        }

        public async Task<IEnumerable<LunchSetDto>> GetAllAsync()
        {
            var lunchSets = await _lunchSetRepo.GetAllWithIncludeAsync(query =>
                query.Include(l => l.LunchSetProducts)
                     .ThenInclude(lp => lp.Product));

            return lunchSets.Select(ls => new LunchSetDto
            {
                Id = ls.Id,
                Title = ls.Title,
                Desc = ls.Desc,
                Price = ls.Price,
                ProductNames = ls.LunchSetProducts.Select(lsp => lsp.Product.Name).ToList()
            }).ToList();
        }

        public async Task<LunchSetDto> GetByIdAsync(int id)
        {
            return _mapper.Map<LunchSetDto>(await _lunchSetRepo.GetByIdAsync(id));
        }

        public async Task<IEnumerable<LunchSetDto>> SearchAsync(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                var allLunchSets = await _lunchSetRepo.GetAllAsync();
                return _mapper.Map<IEnumerable<LunchSetDto>>(allLunchSets);
            }
            var lunchSets = await _lunchSetRepo.GetAllWithExpression(c =>
                c.Title.Contains(str) || c.Desc.Contains(str)
            );

            if (!lunchSets.Any())
            {
                throw new NotFoundException("No lunchSets found matching the search criteria.");
            }

            return _mapper.Map<IEnumerable<LunchSetDto>>(lunchSets);
        }


        public async Task EditAsync(int id, LunchSetEditDto lunchSet)
        {
            var existingLunchSet = await _lunchSetRepo.GetByIdAsync(id);
            if (existingLunchSet == null)
            {
                throw new NotFoundException("LunchSet not found");
            }

            var duplicateLunchSet = await _lunchSetRepo.GetAllWithExpression(
                x => x.Desc == (lunchSet.Desc ?? existingLunchSet.Desc) &&
                    x.Title == (lunchSet.Title ?? existingLunchSet.Title) &&
                     x.Id != id
            );

            if (duplicateLunchSet.Any())
            {
                throw new ArgumentException("An lunchSet with the same desc or title already exists.");
            }

            existingLunchSet.Desc = string.IsNullOrWhiteSpace(lunchSet.Desc) ? existingLunchSet.Desc : lunchSet.Desc;
            existingLunchSet.Title = string.IsNullOrWhiteSpace(lunchSet.Title) ? existingLunchSet.Title : lunchSet.Title;

            await _lunchSetRepo.EditAsync(existingLunchSet);
        }

    }
}
