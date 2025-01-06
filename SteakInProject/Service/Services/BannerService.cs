using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.Helpers.DTOs.Banner;
using Service.Helpers.DTOs.Slider;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class BannerService:IBannerService
	{
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public BannerService(AppDbContext context,
                             IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<string> CreateAsync(BannerCreateDto banner)
        {
            var fileResponse = await _fileService.UploadAsync(banner.File);

            if (fileResponse.HasError == true)
            {
                return fileResponse.Response;
            }

            var newBanner = new Banner
            {
                ImgUrl = banner.ImgUrl,
                AltText = banner.AltText,
                Image = fileResponse.Response
            };

            await _context.Banners.AddAsync(newBanner);
            await _context.SaveChangesAsync();

            return "Success";
        }

        public async Task<string> DeleteAsync(int id)
        {
            var findData = await _context.Banners.FindAsync(id);

            if (findData == null)
            {
                return "Data not found";
            }
            await _fileService.DeletePath(findData.Image);
            _context.Banners.Remove(findData);
            await _context.SaveChangesAsync();

            return "Success";
        }


        public async Task<string> EditAsync(int id, BannerEditDto banner)
        {
            var findBanner = await _context.Banners.FindAsync(id);

            if (findBanner == null)
            {
                return "Data not found";
            }
            if (!string.IsNullOrEmpty(banner.AltText))
            {
                findBanner.AltText = banner.AltText;
            }
            if (!string.IsNullOrEmpty(banner.ImgUrl))
            {
                findBanner.ImgUrl = banner.ImgUrl;
            }
            if (banner.file != null)
            {
                await _fileService.DeletePath(findBanner.Image);
                var fileResponse = await _fileService.UploadAsync(banner.file);
                if (fileResponse.HasError)
                {
                    return fileResponse.Response;
                }
                findBanner.Image = fileResponse.Response;
            }
            await _context.SaveChangesAsync();

            return "Success";
        }


        public async Task<ICollection<Banner>> GetAllAsync()
        {
            var datas = await _context.Banners.ToListAsync();

            return datas;
        }

        public Task<Banner> GetById(int id)
        {
            return _context.Banners.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

