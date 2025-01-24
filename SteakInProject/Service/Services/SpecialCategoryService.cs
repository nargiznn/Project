using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.SpecialCategory;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class SpecialCategoryService:ISpecialCategoryService
	{
        private readonly ISpecialCategoryRepository _specialCategoryRepo;
        private readonly IMapper _mapper;
        public SpecialCategoryService(ISpecialCategoryRepository specialCategoryRepository,
                                                              IMapper mapper)
        {
            _specialCategoryRepo = specialCategoryRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(SpecialCategoryCreateDto specialCategory)
        {

            var existingSpecialCategory = await _specialCategoryRepo.GetAllWithExpression(
                x => x.Name == specialCategory.Name 
            );
            if (existingSpecialCategory.Any())
            {
                throw new ArgumentException("An specialCategory with the same name already exists.");
            }

            await _specialCategoryRepo.CreateAsync(_mapper.Map<SpecialCategory>(specialCategory));
        }

        public async Task DeleteAsync(int id)
        {
            await _specialCategoryRepo.DeleteAsync(id);
        }

        public async Task<IEnumerable<SpecialCategoryDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<SpecialCategoryDto>>(await _specialCategoryRepo.GetAllAsync());
        }

        public async Task<SpecialCategoryDto> GetByIdAsync(int id)
        {
            return _mapper.Map<SpecialCategoryDto>(await _specialCategoryRepo.GetByIdAsync(id));
        }

        public async Task<IEnumerable<SpecialCategoryDto>> SearchAsync(string str)
        {
            var specialCategorys = await _specialCategoryRepo.GetAllWithExpression(c =>
                c.Name.Contains(str) 
            );

            if (!specialCategorys.Any())
            {
                throw new NotFoundException("No SpecialCategory found matching the search criteria.");
            }

            return _mapper.Map<IEnumerable<SpecialCategoryDto>>(specialCategorys);
        }

        public async Task EditAsync(int id, SpecialCategoryEditDto specialCategory)
        {
            var existingSpecialCategory = await _specialCategoryRepo.GetByIdAsync(id);
            if (existingSpecialCategory == null)
            {
                throw new NotFoundException("SpecialCategory not found");
            }
            var duplicateSpecialCategory = await _specialCategoryRepo.GetAllWithExpression(
                x => x.Name == (specialCategory.Name ?? existingSpecialCategory.Name) &&
                     x.Id != id
            );

            if (duplicateSpecialCategory.Any())
            {
                throw new ArgumentException("An SpecialCategory with the same name already exists.");
            }

            existingSpecialCategory.Name = string.IsNullOrWhiteSpace(specialCategory.Name) ? existingSpecialCategory.Name : specialCategory.Name;
            await _specialCategoryRepo.EditAsync(existingSpecialCategory);
        }
    }
}

