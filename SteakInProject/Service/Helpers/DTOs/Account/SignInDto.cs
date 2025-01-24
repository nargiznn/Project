using System;
namespace Service.Helpers.DTOs.Account
{
	public class SignInDto
	{
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}

