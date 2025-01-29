using System;
namespace SteakInMCV.Helpers.Response
{
	public class SignUpResponse
	{
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}

