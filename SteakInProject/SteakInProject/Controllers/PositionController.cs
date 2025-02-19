using System;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.DTOs.Position;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
	public class PositionController:BaseController
	{
        private readonly IPositionService _service;
        public PositionController(IPositionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PositionCreateDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _service.CreateAsync(request);

            if (response != "Success") return BadRequest(response);

            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] PositionEditDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _service.EditAsync(id, request);

            if (response != "Success")
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var position = await _service.GetById(id);

            if (position == null)
            {
                return NotFound("Data not found");
            }
            return Ok(position);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _service.DeleteAsync(id);

            if (response == "Data not found")
            {

                return NotFound(response);
            }
            return Ok(response);
        }
    }
}

