using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Data;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.Award;
using Service.Helpers.DTOs.Chef;
using Service.Helpers.DTOs.Position;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class PositionService: IPositionService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public PositionService(AppDbContext ccontext,
                           IMapper mapper)
        {
            _context = ccontext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PositionDto>> GetAllAsync()
        {
            var positions = await _context.Positions
                .Include(x => x.ChefPosition)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<PositionDto>>(positions);
        }

        public async Task<string> DeleteAsync(int id)
        {
            var findData = await _context.Positions.FindAsync(id);

            if (findData == null)
            {
                return "Data not found";
            }
            _context.Positions.Remove(findData);
            await _context.SaveChangesAsync();

            return "Success";
        }

        public async Task<string> CreateAsync(PositionCreateDto position)
        {
            var title = position.Title?.Trim();
            var description = position.Description?.Trim();

            var existingPosition = await _context.Positions
                .FirstOrDefaultAsync(x => x.Title == title);

            if (existingPosition != null)
            {
                return "Eyni title ilə mövcud data var.";
            }

            var newPosition = new Position
            {
                Title = title,
                Description = description,
                IsActive = position.IsActive
            };

            await _context.Positions.AddAsync(newPosition);
            await _context.SaveChangesAsync();

            return "Success";
        }

        public async Task<string> EditAsync(int id, PositionEditDto position)
        {
            var findPosition = await _context.Positions.FindAsync(id);

            if (findPosition == null)
            {
                return "Data not found";
            }

            var title = position.Title?.Trim();
            var description = position.Description?.Trim();

            if (!string.IsNullOrEmpty(title))
            {
                bool isDuplicateTitle = await _context.Positions.AnyAsync(x => x.Id != id && x.Title == title);
                if (isDuplicateTitle)
                {
                    return "Eyni title ilə mövcud data var.";
                }

                findPosition.Title = title;
            }

            if (!string.IsNullOrEmpty(description))
            {
                findPosition.Description = description;
            }

            if (position.IsActive.HasValue)
            {
                findPosition.IsActive = position.IsActive.Value;
            }

            await _context.SaveChangesAsync();

            return "Success";
        }




        public Task<Position> GetById(int id)
        {
            return _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}

