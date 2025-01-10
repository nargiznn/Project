using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repository.Data;
using Service.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInProject.Controllers
{
    public class MealPackageController : BaseController
    {
        private readonly IMealPackageService _mealPackageService;
        private readonly AppDbContext _context;
        public MealPackageController(IMealPackageService mealPackageService, AppDbContext context)
        {
            _mealPackageService = mealPackageService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mealPackageService.GetAllAsync());
        }
    }
}

