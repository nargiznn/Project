using System;
using Domain.Common;
using Domain.Entities;
using Service.Helpers.DTOs.SocialMediaLink;

namespace Service.Helpers.DTOs.Chef
{
	public class ChefDto:BaseEntity
	{
		public string Name { get; set; }
		public string Surname { get; set; }
        public IEnumerable<string> Positions { get; set; }
        public IEnumerable<string> Images { get; set; }
        public SocialMediaLinkDto SocialMedia { get; set; }
    }
}

