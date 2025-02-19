using System;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.Event;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
	public class EventController:BaseController
	{
        private readonly IEventService _service;

        public EventController(IEventService service)
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
                var eventItem = await _service.GetByIdAsync(id);
                return Ok(eventItem);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EventCreateDto request)
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
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] EventEditDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Event data cannot be null.");
                }

                await _service.EditAsync(id, request);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound("Event not found.");
            }

        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            try
            {
                var events = await _service.SearchAsync(keyword);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }




    }
}

