using System;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.MenuCategory;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers.Admin
{
	public class MenuCategoryController:MainController
	{
        private readonly IMenuCategoryService _menuCategoryService;
        public MenuCategoryController(IMenuCategoryService menuCategoryService)
        {
            _menuCategoryService = menuCategoryService;
        }
        [ProducesResponseType(typeof(MenuCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MenuCategoryDto), StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(typeof(MenuCategoryDto), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MenuCategoryCreateDto request)
        {
            await _menuCategoryService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Succesfully");
        }
        [ProducesResponseType(typeof(MenuCategoryDto), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await _menuCategoryService.GetAllAsync());
        }
        [ProducesResponseType(typeof(MenuCategoryDto), StatusCodes.Status200OK)]
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
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string searchString)
        {
            var categories = await _menuCategoryService.SearchAsync(searchString);
            return Ok(categories);
        }
        [ProducesResponseType(typeof(MenuCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MenuCategoryDto), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] MenuCategoryEditDto request)
        {
            try
            {
                await _menuCategoryService.EditAsync(id, request);
                return Ok("MenuCategor updated successfully");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}

