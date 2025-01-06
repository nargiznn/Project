using System;
namespace Service.Helpers.Responses
{
	public class SignUpResponse
	{
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}

