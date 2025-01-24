using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.Cuisine;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class CuisineService:ICuisineService
	{
        private readonly ICuisineRepository _cuisineRepo;
        private readonly IMapper _mapper;
        public CuisineService(ICuisineRepository cuisineRepository, IMapper mapper)
        {
            _cuisineRepo = cuisineRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(CuisineCreateDto cuisine)
        {
            var existingCuisine = await _cuisineRepo.GetAllWithExpression(
                x => x.Name == cuisine.Name && x.Desc == cuisine.Desc
            );
            if (existingCuisine.Any())
            {
                throw new ArgumentException("An Cuisine with the same name and desc already exists.");
            }

            await _cuisineRepo.CreateAsync(_mapper.Map<Cuisine>(cuisine));
        }

        public async Task DeleteAsync(int id)
        {
            await _cuisineRepo.DeleteAsync(id);
        }

        public async Task<IEnumerable<CuisineDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CuisineDto>>(await _cuisineRepo.GetAllAsync());
        }

        public async Task<CuisineDto> GetByIdAsync(int id)
        {
            return _mapper.Map<CuisineDto>(await _cuisineRepo.GetByIdAsync(id));
        }

        public async Task<IEnumerable<CuisineDto>> SearchAsync(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                var allCuisines = await _cuisineRepo.GetAllAsync();
                return _mapper.Map<IEnumerable<CuisineDto>>(allCuisines);
            }
            var cuisines = await _cuisineRepo.GetAllWithExpression(c =>
                c.Name.Contains(str) || c.Desc.Contains(str)
            );

            if (!cuisines.Any())
            {
                throw new NotFoundException("No Cuisines found matching the search criteria.");
            }

            return _mapper.Map<IEnumerable<CuisineDto>>(cuisines);
        }


        public async Task EditAsync(int id, CuisineEditDto cuisine)
        {
            var existingCuisine = await _cuisineRepo.GetByIdAsync(id);
            if (existingCuisine == null)
            {
                throw new NotFoundException("Cuisine not found");
            }

            var duplicateCuisine = await _cuisineRepo.GetAllWithExpression(
                x => x.Name == (cuisine.Name ?? existingCuisine.Name) &&
                 x.Desc == (cuisine.Desc ?? existingCuisine.Desc) &&
                     x.Id != id
            );


            if (duplicateCuisine.Any())
            {
                throw new ArgumentException("An Cuisine with the same name and desc already exists.");
            }

            existingCuisine.Name = string.IsNullOrWhiteSpace(cuisine.Name) ? existingCuisine.Name : cuisine.Name;
            existingCuisine.Desc = string.IsNullOrWhiteSpace(cuisine.Desc) ? existingCuisine.Desc : cuisine.Desc;

            await _cuisineRepo.EditAsync(existingCuisine);
        }
    }
}

