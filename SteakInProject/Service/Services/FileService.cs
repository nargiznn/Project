using System;
using Microsoft.AspNetCore.Http;
using Repository.Data;
using Service.Helpers.Responses;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class FileService : IFileService
    {
        private readonly AppDbContext _context;
        public FileService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UploadResponse> DeletePath(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            return new UploadResponse { HasError = false, Response = "Success" };
        }
        public async Task<UploadResponse> UploadAsync(IFormFile file)
        {
            List<string> validExtentions = new() { ".jpg", ".png", ".gif" };
            string fileExtention = Path.GetExtension(file.FileName);
            if (!validExtentions.Contains(fileExtention))
            {
                return new UploadResponse { HasError = true, Response = $"File extention is wrong!(valid extentions : ({string.Join(",", validExtentions)}))" };

            }
            long size = file.Length;
            if (size > 2 * 1024 * 1024)
            {
                return new UploadResponse { HasError = true, Response = "file size can be max 2mb" };

            }
            string fileName = Guid.NewGuid().ToString() + fileExtention;

            if (!Directory.Exists("wwwroot//Uploads"))
            {
                Directory.CreateDirectory("wwwroot//Uploads");
            }

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//Uploads", fileName);

            using FileStream stream = new(path, FileMode.Create);

            await file.CopyToAsync(stream);
            return new UploadResponse { HasError = false, Response = fileName };
        }
    }
}

