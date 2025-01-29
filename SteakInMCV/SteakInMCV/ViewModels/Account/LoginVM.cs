using System;
using System.ComponentModel.DataAnnotations;

namespace SteakInMCV.ViewModels.Account
{
	public class LoginVM
	{
        [Required]
        public string usernameOrEmail { get; set; }
        [Required]
        public string password { get; set; }
    }
}

