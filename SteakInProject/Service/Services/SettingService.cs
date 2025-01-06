using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.Setting;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class SettingService:ISettingService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SettingService(AppDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task EditAsync(int id, SettingEditDto setting)
        {
            var existSetting = await _context.Settings.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id) ?? throw new NotFoundException("Data notfound");

            _mapper.Map(setting, existSetting);

            _context.Settings.Update(existSetting);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SettingDto>> GetAllAsync()
        {
            return _mapper.Map<List<SettingDto>>(await _context.Settings.AsNoTracking().ToListAsync());
        }

        public async Task<SettingDto> GetByIdAsync(int id)
        {
            var result = await _context.Settings.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (result is null) return null;

            return _mapper.Map<SettingDto>(result);
        }
    }
}

