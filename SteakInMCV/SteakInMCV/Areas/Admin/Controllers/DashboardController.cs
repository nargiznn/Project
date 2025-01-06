using System;
using Microsoft.AspNetCore.Mvc;

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController:Controller
	{
        public IActionResult Index()
        {
            return View();
        }
    }
}

