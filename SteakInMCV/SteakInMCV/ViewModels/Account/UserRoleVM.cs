using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SteakInMCV.ViewModels.Account
{
	public class UserRoleVM
	{
        [Required(ErrorMessage = "User selection is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Role selection is required.")]
        public string RoleId { get; set; }

        public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>();
    }
}

