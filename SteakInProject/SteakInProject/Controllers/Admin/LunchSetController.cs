using System;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.LunchSet;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers.Admin
{
	public class LunchSetController:MainController
	{
        private readonly ILunchSetService _lunchSetService;
        public LunchSetController(ILunchSetService lunchSetService)
        {
            _lunchSetService = lunchSetService;
        }
        [ProducesResponseType(typeof(LunchSetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(LunchSetDto), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _lunchSetService.GetByIdAsync(id));

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        [ProducesResponseType(typeof(LunchSetDto), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LunchSetCreateDto request)
        {
            await _lunchSetService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Succesfully");
        }
        [ProducesResponseType(typeof(LunchSetDto), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await _lunchSetService.GetAllAsync());
        }
        [ProducesResponseType(typeof(LunchSetDto), StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {

                await _lunchSetService.DeleteAsync(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string searchString)
        {
            var categories = await _lunchSetService.SearchAsync(searchString);
            return Ok(categories);
        }
        [ProducesResponseType(typeof(LunchSetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(LunchSetDto), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] LunchSetEditDto request)
        {
            try
            {
                await _lunchSetService.EditAsync(id, request);
                return Ok("LunchSet updated successfully");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}

