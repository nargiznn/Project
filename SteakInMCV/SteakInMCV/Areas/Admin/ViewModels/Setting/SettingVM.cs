using System;
namespace SteakInMCV.Areas.Admin.ViewModels.Setting
{
	public class SettingVM
	{
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string? Image { get; set; }
    }
}

