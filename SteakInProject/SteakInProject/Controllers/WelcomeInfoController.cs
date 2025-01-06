using System;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.WelcomeInfo;
using Service.Services;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
	public class WelcomeInfoController:BaseController
	{
        private readonly IWelcomeInfoService _welcomeInfoService;
        public WelcomeInfoController(IWelcomeInfoService welcomeInfoService)
        {
            _welcomeInfoService = welcomeInfoService;
        }
        [ProducesResponseType(typeof(WelcomeInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WelcomeInfoDto), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _welcomeInfoService.GetByIdAsync(id));

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        [ProducesResponseType(typeof(WelcomeInfoDto), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WelcomeInfoCreateDto request)
        {
            await _welcomeInfoService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Succesfully");
        }
        [ProducesResponseType(typeof(WelcomeInfoDto), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await _welcomeInfoService.GetAllAsync());
        }
        [ProducesResponseType(typeof(WelcomeInfoDto), StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {

                await _welcomeInfoService.DeleteAsync(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }
        [ProducesResponseType(typeof(WelcomeInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WelcomeInfoDto), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] WelcomeInfoEditDto request)
        {
            try
            {
                await _welcomeInfoService.EditAsync(id, request);
                return Ok("WelcomeInfoDto updated successfully");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

    }
}

