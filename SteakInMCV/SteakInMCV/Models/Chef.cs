using System;
namespace SteakInMCV.Models
{
	public class Chef
	{
        public string Name { get; set; }
        public string Surname { get; set; }
        public SocialMediaLink SocialMedia { get; set; }
        public ICollection<string> Positions { get; set; } = new List<string>();
        public ICollection<string> Images { get; set; } = new List<string>();
    }
}

