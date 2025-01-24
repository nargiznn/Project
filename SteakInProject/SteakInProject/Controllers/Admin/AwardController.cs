using System;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.Award;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers.Admin
{
	public class AwardController:MainController
	{
        private readonly IAwardService _awardService;
        public AwardController(IAwardService awardService)
        {
            _awardService = awardService;
        }
        [ProducesResponseType(typeof(AwardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AwardDto), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _awardService.GetByIdAsync(id));

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        [ProducesResponseType(typeof(AwardDto), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AwardCreateDto request)
        {
            await _awardService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Succesfully");
        }
        [ProducesResponseType(typeof(AwardDto), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await _awardService.GetAllAsync());
        }
        [ProducesResponseType(typeof(AwardDto), StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {

                await _awardService.DeleteAsync(id);
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
            var categories = await _awardService.SearchAsync(searchString);
            return Ok(categories);
        }
        [ProducesResponseType(typeof(AwardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AwardDto), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] AwardEditDto request)
        {
            try
            {
                await _awardService.EditAsync(id, request);
                return Ok("Award updated successfully");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}

