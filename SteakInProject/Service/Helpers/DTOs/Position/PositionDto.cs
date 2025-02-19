using System;
using Domain.Entities;

namespace Service.Helpers.DTOs.Position
{
	public class PositionDto
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}

