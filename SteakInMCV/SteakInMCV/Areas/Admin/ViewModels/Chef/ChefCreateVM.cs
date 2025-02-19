using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using SteakInMCV.Areas.Admin.ViewModels.SocialMediaLink;

namespace SteakInMCV.Areas.Admin.ViewModels.Chef
{
	public class ChefCreateVM
	{
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        public string Surname { get; set; }

        public IFormFile Photos { get; set; }

        [Required(ErrorMessage = "At least one position must be selected.")]
        public List<int> SelectedPositions { get; set; }

        public List<SelectListItem> AvailablePositions { get; set; } = new();

        public SocialMediaLinkCreateDto SocialMedia { get; set; } = new SocialMediaLinkCreateDto();

    }
}

