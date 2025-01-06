using System;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.FoodCategory;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
	public class FoodCategoryController: BaseController
    {
        private readonly IFoodCategoryService _foodCategoryService;
        public FoodCategoryController(IFoodCategoryService foodCategoryService)
        {
            _foodCategoryService = foodCategoryService;
        }
        [ProducesResponseType(typeof(FoodCategory), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FoodCategory), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _foodCategoryService.GetByIdAsync(id));

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        [ProducesResponseType(typeof(FoodCategory), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FoodCategoryCreateDto request)
        {
            await _foodCategoryService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Succesfully");
        }
        [ProducesResponseType(typeof(FoodCategory), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await _foodCategoryService.GetAllAsync());
        }
        [ProducesResponseType(typeof(FoodCategory), StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {

                await _foodCategoryService.DeleteAsync(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }
        [ProducesResponseType(typeof(FoodCategory), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FoodCategory), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] FoodCategoryEditDto request)
        {
            try
            {
                await _foodCategoryService.EditAsync(id, request);
                return Ok("FoodCategory updated successfully");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}

