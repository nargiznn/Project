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
         private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var eventDto = await _eventService.GetByIdAsync(id);
                return Ok(eventDto);
            }
            catch (NotFoundException)
            {
                return NotFound("Event not found");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _eventService.GetAllAsync();
            return Ok(events);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EventCreateDto request)
        {
            await _eventService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Event created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] EventEditDto request)
        {
            try
            {
                await _eventService.EditAsync(id, request);
                return Ok("Event updated successfully");
            }
            catch (NotFoundException)
            {
                return NotFound("Event not found");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _eventService.DeleteAsync(id);
                return Ok("Event deleted successfully");
            }
            catch (NotFoundException)
            {
                return NotFound("Event not found");
            }
        }
    }
}

