using System;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.DTOs.Subscribe;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers.Admin
{
	public class SubscribeController:MainController
	{
        private readonly ISubscribeService _subscribeService;
        public SubscribeController(ISubscribeService subscribeService)
        {
            _subscribeService = subscribeService;
        }


        [ProducesResponseType(typeof(SubscribeDto), StatusCodes.Status201Created)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _subscribeService.GetAllAsync());
        }
    }
}

