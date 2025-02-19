using System;
namespace SteakInMCV.Areas.Admin.ViewModels.Position
{
	public class PositionCreateVM
	{
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
    }
}

