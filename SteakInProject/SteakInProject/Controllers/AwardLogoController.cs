using System;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.AwardLogo;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
	public class AwardLogoController:BaseController
	{
        private readonly IAwardLogoService _service;
        public AwardLogoController(IAwardLogoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var logo = await _service.GetByIdAsync(id);
                return Ok(logo);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AwardLogoCreateDto request)
        {

            try
            {
                await _service.CreateAsync(request);
                return CreatedAtAction(nameof(Create), "Successfully created");
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] AwardLogoEditDto request)
        {
            try
            {
                await _service.EditAsync(id, request);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }
    }
}

