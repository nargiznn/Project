using System;
namespace Service.Helpers.DTOs.Position
{
	public class PositionCreateDto
	{
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}

