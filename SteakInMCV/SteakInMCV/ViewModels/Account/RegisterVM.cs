using System;
using System.ComponentModel.DataAnnotations;

namespace SteakInMCV.ViewModels.Account
{
	public class RegisterVM
	{
        [Required]
        public string fullName { get; set; }
        [Required]
        public string userName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}

