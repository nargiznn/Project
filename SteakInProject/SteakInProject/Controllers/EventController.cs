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
        private readonly AppDbContext _context;
        public EventController(IEventService eventService, AppDbContext context)
        {
            _eventService = eventService;
            _context = context;
        }

        [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _eventService.GetByIdAsync(id));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }


        [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _eventService.GetAllAsync());
        }

        [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _eventService.DeleteAsync(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("tags")]
        [ProducesResponseType(typeof(List<Tag>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTags()
        {
            return Ok(await _context.Tags.AsNoTracking().ToListAsync());
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] EventEditDto request)
        {
            try
            {
                await _eventService.EditAsync(id, request);
                return Ok("Event updated successfully");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        [HttpPost]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] EventCreateDto request)
        {
            await _eventService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Event created successfully");
        }
    }
}

