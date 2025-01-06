using System;
namespace Service.Helpers.Responses
{
	public class SignInResponse
	{
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string Token { get; set; }
    }
}

