using System;
namespace Service.Helpers.Responses
{
	public class VerificationResponse
	{
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}

