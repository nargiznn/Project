using System;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers.DTOs.Setting
{
	public class SettingEditDto
	{
        public string? Value { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}

