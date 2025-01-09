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
    public class ClientController : BaseController
    {
        private readonly IClientService _clientService;
        private readonly AppDbContext _context;
        public ClientController(IClientService clientService, AppDbContext context)
        {
            _clientService = clientService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _clientService.GetAllAsync());
        }
    }
}

