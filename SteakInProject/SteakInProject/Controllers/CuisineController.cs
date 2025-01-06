using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.Cuisine;
using Service.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInProject.Controllers
{
    public class CuisineController : BaseController
    {
        private readonly ICuisineService _cusineService;
        public CuisineController(ICuisineService cuisineService)
        {
            _cusineService = cuisineService;
        }
        [ProducesResponseType(typeof(CuisineDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CuisineDto), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _cusineService.GetByIdAsync(id));

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
            await _cusineService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Succesfully");
        }
        [ProducesResponseType(typeof(CuisineDto), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await _cusineService.GetAllAsync());
        }
        [ProducesResponseType(typeof(CuisineDto), StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {

                await _cusineService.DeleteAsync(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }
        [ProducesResponseType(typeof(CuisineDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CuisineDto), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] CuisineEditDto request)
        {
            try
            {
                await _cusineService.EditAsync(id, request);
                return Ok("Cuisine updated successfully");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}

