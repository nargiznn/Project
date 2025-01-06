using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Data;
using Service.Helpers.DTOs.WelcomeImage;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class WelcomeImageService:IWelcomeImageService
	{
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public WelcomeImageService(AppDbContext context,
                             IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<string> CreateAsync(WelcomeImageCreateDto welcomeImage)
        {
            var fileResponse = await _fileService.UploadAsync(welcomeImage.File);

            if (fileResponse.HasError == true)
            {
                return fileResponse.Response;
            }

            var newWelcomeImage = new WelcomeImage
            {
                Image = fileResponse.Response
            };

            await _context.WelcomeImages.AddAsync(newWelcomeImage);
            await _context.SaveChangesAsync();

            return "Success";
        }

        public async Task<string> DeleteAsync(int id)
        {
            var findData = await _context.WelcomeImages.FindAsync(id);

            if (findData == null)
            {
                return "Data not found";
            }
            await _fileService.DeletePath(findData.Image);
            _context.WelcomeImages.Remove(findData);
            await _context.SaveChangesAsync();

            return "Success";
        }


        public async Task<string> EditAsync(int id, WelcomeImageEditDto welcomeImage)
        {
            var findWelcomeImage = await _context.WelcomeImages.FindAsync(id);

            if (findWelcomeImage == null)
            {
                return "Data not found";
            }

            // IsMain sahəsi dəyişirsə
            if (welcomeImage.IsMain.HasValue)
            {
                findWelcomeImage.IsMain = welcomeImage.IsMain.Value;
            }

            // File (şəkil) sahəsi dəyişirsə
            if (welcomeImage.File != null)
            {
                // Əvvəlki şəkili silirik
                await _fileService.DeletePath(findWelcomeImage.Image);

                // Yeni şəkil yükləyirik
                var fileResponse = await _fileService.UploadAsync(welcomeImage.File);
                if (fileResponse.HasError)
                {
                    return fileResponse.Response;
                }

                // Yeni şəkilin yolunu saxlayırıq
                findWelcomeImage.Image = fileResponse.Response;
            }

            // Dəyişiklikləri saxlayırıq
            await _context.SaveChangesAsync();

            return "Success";
        }



        public async Task<ICollection<WelcomeImage>> GetAllAsync()
        {
            var datas = await _context.WelcomeImages.ToListAsync();

            return datas;
        }

        public Task<WelcomeImage> GetById(int id)
        {
            return _context.WelcomeImages.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

