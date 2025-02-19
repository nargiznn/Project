using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.Helpers.DTOs.Slider;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class SliderService:ISliderService
	{
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public SliderService(AppDbContext context,
                             IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<string> DeleteAsync(int id)
        {
            var findData = await _context.Sliders.FindAsync(id);

            if (findData == null)
            {
                return "Data not found";
            }
            await _fileService.DeletePath(findData.Image);
            _context.Sliders.Remove(findData);
            await _context.SaveChangesAsync();

            return "Success";
        }


        public async Task<string> CreateAsync(SliderCreateDto slider)
        {
            var fileResponse = await _fileService.UploadAsync(slider.File);

            if (fileResponse.HasError)
            {
                return fileResponse.Response;
            }

            var newSlider = new Slider
            {
                Title = slider.Title,
                MainTitle = slider.MainTitle,
                Desc = slider.Desc,
                BtnText = slider.BtnText,
                Image = $"http://localhost:7031/uploads/{fileResponse.Response}"
            };

            await _context.Sliders.AddAsync(newSlider);
            await _context.SaveChangesAsync();

            return "Success";
        }

        public async Task<string> EditAsync(int id, SliderEditDto slider)
        {
            var findSlider = await _context.Sliders.FindAsync(id);

            if (findSlider == null)
            {
                return "Data not found";
            }

            if (!string.IsNullOrEmpty(slider.Title))
            {
                findSlider.Title = slider.Title;
            }
            if (!string.IsNullOrEmpty(slider.MainTitle))
            {
                findSlider.MainTitle = slider.MainTitle;
            }
            if (!string.IsNullOrEmpty(slider.Desc))
            {
                findSlider.Desc = slider.Desc;
            }
            if (!string.IsNullOrEmpty(slider.BtnText))
            {
                findSlider.BtnText = slider.BtnText;
            }

            if (slider.file != null)
            {
                await _fileService.DeletePath(findSlider.Image);

                var fileResponse = await _fileService.UploadAsync(slider.file);
                if (fileResponse.HasError)
                {
                    return fileResponse.Response;
                }
                findSlider.Image = $"http://localhost:7031/uploads/{fileResponse.Response}";
            }

            await _context.SaveChangesAsync();

            return "Success";
        }



        public async Task<ICollection<Slider>> GetAllAsync()
        {
            var datas = await _context.Sliders.ToListAsync();

            return datas;
        }

        public Task<Slider> GetById(int id)
        {
            return _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

