using System;
namespace Service.Helpers.Responses
{
	public class ResetPasswordResponse
	{
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}

