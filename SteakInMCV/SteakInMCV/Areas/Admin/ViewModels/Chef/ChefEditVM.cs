using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SteakInMCV.Areas.Admin.ViewModels.SocialMediaLink;

namespace SteakInMCV.Areas.Admin.ViewModels.Chef
{
    public class ChefEditVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public SocialMediaLinkDto? SocialMedia { get; set; }
        public List<IFormFile>? Photos { get; set; }
        public List<int>? SelectedPositions { get; set; }

        public List<SelectListItem>? AvailablePositions { get; set; } = new();
    }
}
