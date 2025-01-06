using System;
using Microsoft.AspNetCore.Http;
using Service.Helpers.Responses;

namespace Service.Services.Interfaces
{
	public interface IFileService
	{
        Task<UploadResponse> UploadAsync(IFormFile file);
        Task<UploadResponse> DeletePath(string fileName);
    }
}

