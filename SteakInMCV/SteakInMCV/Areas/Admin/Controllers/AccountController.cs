using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.Account;
using SteakInMCV.Areas.Admin.ViewModels.Tag;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Index()
        {
            IEnumerable<AccountVM> categoryVMs = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/account/getusers"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVMs = JsonConvert.DeserializeObject<IEnumerable<AccountVM>>(apiResponse);
                }
            }
            return View(categoryVMs);
        }

    }
}

