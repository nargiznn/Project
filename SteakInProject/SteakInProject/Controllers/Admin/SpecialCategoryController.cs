using System;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.SpecialCategory;
using Service.Services;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers.Admin
{
	public class SpecialCategoryController:MainController
	{
        private readonly ISpecialCategoryService _specialCateService;
        public SpecialCategoryController(ISpecialCategoryService specialCategService)
        {
            _specialCateService = specialCategService;
        }
        [ProducesResponseType(typeof(SpecialCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SpecialCategoryDto), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _specialCateService.GetByIdAsync(id));

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        [ProducesResponseType(typeof(SpecialCategoryDto), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SpecialCategoryCreateDto request)
        {
            await _specialCateService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Succesfully");
        }
        [ProducesResponseType(typeof(SpecialCategoryDto), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _specialCateService.GetAllAsync());
        }
        [ProducesResponseType(typeof(SpecialCategoryDto), StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {

                await _specialCateService.DeleteAsync(id);
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
            var categories = await _specialCateService.SearchAsync(searchString);
            return Ok(categories);
        }
        [ProducesResponseType(typeof(SpecialCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SpecialCategoryDto), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] SpecialCategoryEditDto request)
        {
            try
            {
                await _specialCateService.EditAsync(id, request);
                return Ok("SpecialCategory updated successfully");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}

