using System;
namespace Service.Helpers
{
	public class ConfirmEmailResponse
	{
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}

