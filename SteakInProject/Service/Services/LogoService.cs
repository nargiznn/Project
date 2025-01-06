using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.Logo;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class AwardLogoService : IAwardLogoService
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public AwardLogoService(AppDbContext context,
                             IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<string> CreateAsync(LogoCreateDto logo)
        {
            var fileResponse = await _fileService.UploadAsync(logo.file);

            if (fileResponse.HasError == true)
            {
                return fileResponse.Response;
            }

            var newLogo = new AwardLogo
            {
                ImgUrl = logo.ImgUrl,
                AltText = logo.AltText,
                Image = fileResponse.Response
            };

            await _context.AwardLogos.AddAsync(newLogo);
            await _context.SaveChangesAsync();

            return "Success";
        }

        public async Task<string> DeleteAsync(int id)
        {
            var findData = await _context.AwardLogos.FindAsync(id);

            if (findData == null)
            {
                return "Data not found";
            }
            await _fileService.DeletePath(findData.Image);
            _context.AwardLogos.Remove(findData);
            await _context.SaveChangesAsync();

            return "Success";
        }


        public async Task<string> EditAsync(int id, LogoEditDto logo)
        {
            var findLogo = await _context.AwardLogos.FindAsync(id);

            if (findLogo == null)
            {
                return "Data not found";
            }
            if (!string.IsNullOrEmpty(logo.ImgUrl))
            {
                findLogo.ImgUrl = logo.ImgUrl;
            }
            if (!string.IsNullOrEmpty(logo.AltText))
            {
                findLogo.AltText = logo.AltText;
            }
            if (logo.file != null)
            {
                await _fileService.DeletePath(findLogo.Image);
                var fileResponse = await _fileService.UploadAsync(logo.file);
                if (fileResponse.HasError)
                {
                    return fileResponse.Response;
                }
                findLogo.Image = fileResponse.Response;
            }
            await _context.SaveChangesAsync();

            return "Success";
        }


        public async Task<ICollection<AwardLogo>> GetAllAsync()
        {
            var datas = await _context.AwardLogos.ToListAsync();

            return datas;
        }

        public Task<AwardLogo> GetById(int id)
        {
            return _context.AwardLogos.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

