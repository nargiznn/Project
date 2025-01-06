using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Chef:BaseEntity
	{
		public string Name { get; set; }
		public string Surname { get; set; }
        public SocialMediaLink SocialMedia { get; set; }
        public ICollection<ChefPosition> ChefPosition { get; set; }
        public ICollection<ChefImage> ChefImages { get; set; }

    }
}

