using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.Award;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class AwardService:IAwardService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AwardService(AppDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(AwardCreateDto award)
        {
            await _context.Awards.AddAsync(_mapper.Map<Award>(award));
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var award = await _context.Awards.FindAsync(id) ?? throw new NotFoundException("Data notfound");
            _context.Awards.Remove(award);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, AwardEditDto award)
        {
            var existAward = await _context.Awards.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id) ?? throw new NotFoundException("Data notfound");

            _mapper.Map(award, existAward);

            _context.Awards.Update(existAward);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AwardDto>> GetAllAsync()
        {
            return _mapper.Map<List<AwardDto>>(await _context.Awards.AsNoTracking().ToListAsync());
        }

        public async Task<AwardDto> GetByIdAsync(int id)
        {
            var result = await _context.Awards.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (result is null) return null;

            return _mapper.Map<AwardDto>(result);
        }
    }
}

