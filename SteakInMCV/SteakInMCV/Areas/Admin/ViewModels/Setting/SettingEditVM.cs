using System;
namespace SteakInMCV.Areas.Admin.ViewModels.Setting
{
	public class SettingEditVM
	{
        public int Id { get; set; }
        public string? Value { get; set; }
        public IFormFile? Image { get; set; }
        public string? ExistingImage { get; set; }
    }
}

