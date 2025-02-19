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
        private readonly IFileService _fileService;

        public SettingService(AppDbContext context, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }


        public async Task EditAsync(int id, SettingEditDto setting)
        {
            var existSetting = await _context.Settings.FirstOrDefaultAsync(m => m.Id == id)
                                 ?? throw new NotFoundException("Data not found");

            if (setting.ImageFile != null) 
            {
                var response = await _fileService.UploadAsync(setting.ImageFile);
                setting.Value = $"http://localhost:7031/uploads/{response.Response}";
            }

            existSetting.Value = setting.Value;

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

