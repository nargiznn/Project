using System;
using System.ComponentModel.DataAnnotations;

namespace SteakInMCV.ViewModels.Account
{
	public class UserPasswordVM
	{
        [Required]
        public string email { get; set; }
        [Required]
        public string token { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string confirmPassword { get; set; }
    }
}

