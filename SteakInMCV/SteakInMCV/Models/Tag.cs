using System;
namespace SteakInMCV.Models
{
	public class Tag:BaseEntity
	{
        public string Name { get; set; }
        public List<Event> Events { get; set; } = new List<Event>();
    }
}

