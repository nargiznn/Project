using System;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.MealPackage;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers.Admin
{
	public class MealPackageController:MainController
	{
        private readonly IMealPackageService _mealPackageService;
        public MealPackageController(IMealPackageService mealPackageService)
        {
            _mealPackageService = mealPackageService;
        }
        [ProducesResponseType(typeof(MealPackageDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MealPackageDto), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _mealPackageService.GetByIdAsync(id));

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        [ProducesResponseType(typeof(MealPackageDto), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MealPackageCreateDto request)
        {
            await _mealPackageService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Succesfully");
        }
        [ProducesResponseType(typeof(MealPackageDto), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await _mealPackageService.GetAllAsync());
        }
        [ProducesResponseType(typeof(MealPackageDto), StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {

                await _mealPackageService.DeleteAsync(id);
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
            var categories = await _mealPackageService.SearchAsync(searchString);
            return Ok(categories);
        }
        [ProducesResponseType(typeof(MealPackageDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MealPackageDto), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] MealPackageEditDto request)
        {
            try
            {
                await _mealPackageService.EditAsync(id, request);
                return Ok("MealPackage updated successfully");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}

