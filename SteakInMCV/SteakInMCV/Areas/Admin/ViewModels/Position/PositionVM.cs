using System;
namespace SteakInMCV.Areas.Admin.ViewModels.Position
{
	public class PositionVM
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}

