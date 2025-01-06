using System;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.SpecialCategory;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
	public class SpecialCategoryController: BaseController
    {
        private readonly ISpecialCategoryService _specialCategoryService;
        public SpecialCategoryController(ISpecialCategoryService specialCategoryService)
        {
            _specialCategoryService = specialCategoryService;
        }
        [ProducesResponseType(typeof(SpecialCategory), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SpecialCategory), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _specialCategoryService.GetByIdAsync(id));

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        [ProducesResponseType(typeof(SpecialCategory), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SpecialCategoryCreateDto request)
        {
            await _specialCategoryService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Succesfully");
        }
        [ProducesResponseType(typeof(SpecialCategory), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await _specialCategoryService.GetAllAsync());
        }
        [ProducesResponseType(typeof(SpecialCategory), StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {

                await _specialCategoryService.DeleteAsync(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }
        [ProducesResponseType(typeof(SpecialCategory), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SpecialCategory), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] SpecialCategoryEditDto request)
        {
            try
            {
                await _specialCategoryService.EditAsync(id, request);
                return Ok("SpecialCategory updated successfully");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}

