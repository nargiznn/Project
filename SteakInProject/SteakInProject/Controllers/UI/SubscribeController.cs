using System;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.DTOs.Subscribe;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers.UI
{
	public class SubscribeController:BaseController
	{
        private readonly ISubscribeService _subscribeService;
        public SubscribeController(ISubscribeService subscribeService)
        {
            _subscribeService = subscribeService;
        }


        [ProducesResponseType(typeof(SubscribeCreateDto), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Create(SubscribeCreateDto subscribeCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            await _subscribeService.AddSubscribeAsync(subscribeCreateDto);
            return CreatedAtAction(nameof(Create), new { Response = "You are successfully subscribed!" });
        }
    }
}

