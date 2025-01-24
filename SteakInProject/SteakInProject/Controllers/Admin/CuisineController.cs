using System;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.Cuisine;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers.Admin
{
	public class CuisineController:MainController
    {
        private readonly ICuisineService _cuisineService;
        public CuisineController(ICuisineService cuisineService)
        {
            _cuisineService = cuisineService;
        }
        [ProducesResponseType(typeof(CuisineDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CuisineDto), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _cuisineService.GetByIdAsync(id));

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        [ProducesResponseType(typeof(CuisineDto), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CuisineCreateDto request)
        {
            await _cuisineService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Succesfully");
        }
        [ProducesResponseType(typeof(CuisineDto), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await _cuisineService.GetAllAsync());
        }
        [ProducesResponseType(typeof(CuisineDto), StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {

                await _cuisineService.DeleteAsync(id);
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
            var categories = await _cuisineService.SearchAsync(searchString);
            return Ok(categories);
        }
        [ProducesResponseType(typeof(CuisineDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CuisineDto), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] CuisineEditDto request)
        {
            try
            {
                await _cuisineService.EditAsync(id, request);
                return Ok("Cuisine updated successfully");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}

