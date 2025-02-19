using System;
using SteakInMCV.Areas.Admin.ViewModels.SocialMediaLink;

namespace SteakInMCV.Areas.Admin.ViewModels.Chef
{
	public class ChefVM
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public IEnumerable<string> Positions { get; set; }
        public IEnumerable<string> Images { get; set; }
        public SocialMediaLinkDto SocialMedia { get; set; }
    }
}

