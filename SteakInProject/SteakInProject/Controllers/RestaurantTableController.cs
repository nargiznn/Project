using System;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.Table;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
	public class RestaurantTableController:BaseController
	{
        private readonly IRestaurantTableService _tableService;
        public RestaurantTableController(IRestaurantTableService tableService)
        {
            _tableService = tableService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tableService.GetAllAsync());
        }
    }
}

