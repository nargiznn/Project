using System;
namespace Service.Helpers.Responses
{
	public class ForgotPasswordResponse
	{
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}

