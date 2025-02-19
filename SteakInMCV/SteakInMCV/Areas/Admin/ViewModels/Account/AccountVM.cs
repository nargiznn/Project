using System;
namespace SteakInMCV.Areas.Admin.ViewModels.Account
{
	public class AccountVM
	{
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}

