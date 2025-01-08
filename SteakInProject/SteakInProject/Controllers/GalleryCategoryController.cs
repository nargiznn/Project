using System;
using Microsoft.AspNetCore.Mvc;
using Repository.Data;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
    public class GalleryCategoryController : BaseController
    {
        private readonly IGalleryCategoryService _gallerycatService;
        private readonly AppDbContext _context;
        public GalleryCategoryController(IGalleryCategoryService gallerycatService, AppDbContext context)
        {
            _gallerycatService = gallerycatService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _gallerycatService.GetAllAsync());
        }
    }
}

