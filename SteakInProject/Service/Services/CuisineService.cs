using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.Cuisine;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class CuisineService:ICuisineService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CuisineService(AppDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(CuisineCreateDto cuisine)
        {
            await _context.Cuisines.AddAsync(_mapper.Map<Cuisine>(cuisine));
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cuisine = await _context.Cuisines.FindAsync(id) ?? throw new NotFoundException("Data notfound");
            _context.Cuisines.Remove(cuisine);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, CuisineEditDto cuisine)
        {
            var existCuisine = await _context.Cuisines.FirstOrDefaultAsync(m => m.Id == id)
                ?? throw new NotFoundException("Data not found");
            _mapper.Map(cuisine, existCuisine);
            _context.Cuisines.Update(existCuisine);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<CuisineDto>> GetAllAsync()
        {
            return _mapper.Map<List<CuisineDto>>(await _context.Cuisines.AsNoTracking().ToListAsync());
        }

        public async Task<CuisineDto> GetByIdAsync(int id)
        {
            var result = await _context.Cuisines.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (result is null) return null;

            return _mapper.Map<CuisineDto>(result);
        }
    }
}

