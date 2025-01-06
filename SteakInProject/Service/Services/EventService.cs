using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.Event;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class EventService : IEventService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EventService(AppDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(EventCreateDto events)
        {
            var newEvent = _mapper.Map<Event>(events);

            // Set Tags based on the provided Tag IDs
            if (events.TagIds != null && events.TagIds.Any())
            {
                newEvent.Tags = await _context.Tags
                                              .Where(t => events.TagIds.Contains(t.Id))
                                              .ToListAsync();
            }

            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, EventEditDto events)
        {
            var existingEvent = await _context.Events
                                              .Include(e => e.Tags)
                                              .FirstOrDefaultAsync(e => e.Id == id)
                                              ?? throw new NotFoundException("Event not found");

            // Map updated fields
            _mapper.Map(events, existingEvent);

            // Update Tags if TagIds are provided
            if (events.TagIds != null)
            {
                existingEvent.Tags = await _context.Tags
                                                   .Where(t => events.TagIds.Contains(t.Id))
                                                   .ToListAsync();
            }

            _context.Events.Update(existingEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventDto>> GetAllAsync()
        {
            var events = await _context.Events
                                        .Include(e => e.Tags) // Include tags
                                        .AsNoTracking()
                                        .ToListAsync();

            return _mapper.Map<List<EventDto>>(events);
        }


        public async Task<EventDto> GetByIdAsync(int id)
        {
            var result = await _context.Events
                                        .Include(e => e.Tags) // Include tags
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(e => e.Id == id);

            if (result is null) throw new NotFoundException("Event not found");

            return _mapper.Map<EventDto>(result);
        }
        public async Task DeleteAsync(int id)
        {
            var menuEvent = await _context.Events.FindAsync(id) ?? throw new NotFoundException("Data notfound");
            _context.Events.Remove(menuEvent);
            await _context.SaveChangesAsync();
        }

    }
}

