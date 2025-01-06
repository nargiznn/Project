using System;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.MenuCategory;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
	public class MenuCategoryController: BaseController
    {
        private readonly IMenuCategoryService _menuCategoryService;
        public MenuCategoryController(IMenuCategoryService menuCategoryService)
        {
            _menuCategoryService = menuCategoryService;
        }
        [ProducesResponseType(typeof(MenuCategory), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MenuCategory), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _menuCategoryService.GetByIdAsync(id));

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        [ProducesResponseType(typeof(MenuCategory), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MenuCategoryCreateDto request)
        {
            await _menuCategoryService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Succesfully");
        }
        [ProducesResponseType(typeof(MenuCategory), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await _menuCategoryService.GetAllAsync());
        }
        [ProducesResponseType(typeof(MenuCategory), StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {

                await _menuCategoryService.DeleteAsync(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }
        [ProducesResponseType(typeof(MenuCategory), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MenuCategory), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] MenuCategoryEditDto request)
        {
            try
            {
                await _menuCategoryService.EditAsync(id, request);
                return Ok("MenuCategory updated successfully");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}

