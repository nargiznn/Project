using System;
namespace Domain.Entities
{
	public class Position
	{
        public int Id { get; set; }
        public string Title { get; set; }  
        public string Description { get; set; } 
        public bool IsActive { get; set; }
        public ICollection<ChefPosition> ChefPosition { get; set; }
    }
}

