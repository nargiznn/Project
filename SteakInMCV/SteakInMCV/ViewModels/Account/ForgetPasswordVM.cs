using System;
using System.ComponentModel.DataAnnotations;

namespace SteakInMCV.ViewModels.Account
{
	public class ForgetPasswordVM
	{
        [Required]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        public string requestScheme { get; set; }
        public string requestHost { get; set; }
    }
}

