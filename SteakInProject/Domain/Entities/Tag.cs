using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Tag:BaseEntity
	{
		public string Name { get; set; }
        public List<Event> Events { get; set; } = new List<Event>();

    }
}

