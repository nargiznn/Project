using System;
namespace SteakInMCV.ViewModels.Account
{
	public class GetUserVM
	{
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}

