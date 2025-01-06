using System;
namespace Domain.Entities
{
	public class ChefPosition
	{
		public int Id { get; set; }
        public int ChefId { get; set; }
        public Chef Chef { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime? EndDate { get; set; }
    }
}

