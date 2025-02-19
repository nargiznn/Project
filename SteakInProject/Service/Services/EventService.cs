using System;
using System.Linq;
using System.Reflection.Metadata;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.Event;
using Service.Helpers.Faqs;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class EventService : IEventService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public EventService(AppDbContext context, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task CreateAsync(EventCreateDto eventDto)
        {
            eventDto.Title = eventDto.Title?.Trim();
            eventDto.Desc = eventDto.Desc?.Trim();
            eventDto.Info = eventDto.Info?.Trim();
            if (string.IsNullOrEmpty(eventDto.Title))
            {
                throw new BadHttpRequestException("Event title is required.");
            }

            if (string.IsNullOrEmpty(eventDto.Desc))
            {
                throw new BadHttpRequestException("Event description is required.");
            }
            var existingEvent = await _context.Events
                                               .FirstOrDefaultAsync(e => e.Title == eventDto.Title);
            if (existingEvent != null)
            {
                throw new BadHttpRequestException("A similar event with the same title already exists.");
            }

            if (eventDto.Image != null)
            {
                var response = await _fileService.UploadAsync(eventDto.Image);
                eventDto.ImgUrl = $"http://localhost:7031/uploads/{response.Response}";
            }

            var mappedEvent = _mapper.Map<Event>(eventDto);

            if (eventDto.TagIds != null && eventDto.TagIds.Any())
            {
                var existingTags = await _context.Tags
                    .Where(t => eventDto.TagIds.Contains(t.Id))
                    .ToListAsync();

                var missingTagIds = eventDto.TagIds.Except(existingTags.Select(t => t.Id)).ToList();
                if (missingTagIds.Any())
                {
                    throw new BadHttpRequestException($"The following Tag IDs do not exist: {string.Join(", ", missingTagIds)}");
                }

                mappedEvent.Tags = existingTags;
            }

            await _context.Events.AddAsync(mappedEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventDto>> GetAllAsync()
        {
            var events = await _context.Events
                .Include(e => e.Tags)
                .Include(e => e.Comments)
                    .ThenInclude(c => c.CommentReplies) 
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task<EventDto> GetByIdAsync(int id)
        {
            var eventEntity = await _context.Events
                .Include(e => e.Tags)
                .Include(e => e.Comments)
                    .ThenInclude(c => c.CommentReplies)  
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new NotFoundException("Event not found");

            return _mapper.Map<EventDto>(eventEntity);
        }

        public async Task DeleteAsync(int id)
        {
            var eventEntity = await _context.Events.FirstOrDefaultAsync(x => x.Id == id)
                                 ?? throw new NotFoundException("Event not found");
            _context.Events.Remove(eventEntity);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, EventEditDto eventDto)
        {
            if (eventDto == null)
            {
                throw new ArgumentNullException(nameof(eventDto), "Event data cannot be null");
            }

            var existingEvent = await _context.Events
                .Include(e => e.Tags)  
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingEvent == null)
            {
                throw new NotFoundException("Event not found");
            }
            if (!string.IsNullOrEmpty(eventDto.Title))
            {
                eventDto.Title = eventDto.Title.Trim();

                var existingEventWithSameTitle = await _context.Events
                    .FirstOrDefaultAsync(e => e.Title == eventDto.Title && e.Id != id); 

                if (existingEventWithSameTitle != null)
                {
                    throw new BadHttpRequestException("An event with the same title already exists.");
                }

                existingEvent.Title = eventDto.Title;
            }

            if (!string.IsNullOrEmpty(eventDto.Desc))
            {
                eventDto.Desc = eventDto.Desc.Trim();
                existingEvent.Desc = eventDto.Desc;
            }
            if (!string.IsNullOrEmpty(eventDto.Info))
            {
                eventDto.Info = eventDto.Info.Trim();
                existingEvent.Info = eventDto.Info;
            }

            if (eventDto.Image != null)
            {
                var response = await _fileService.UploadAsync(eventDto.Image);
                existingEvent.ImgUrl = $"http://localhost:7031/uploads/{response.Response}";
            }
            if (eventDto.TagIds != null && eventDto.TagIds.Any())
            {
                var existingTags = await _context.Tags
                    .Where(t => eventDto.TagIds.Contains(t.Id))
                    .ToListAsync();

                var newTags = eventDto.TagIds
                    .Where(id => !existingTags.Any(t => t.Id == id))
                    .Select(id => new Tag { Id = id })
                    .ToList();

                foreach (var tag in existingTags)
                {
                    _context.Entry(tag).State = EntityState.Detached;
                }
                existingEvent.Tags = existingTags.Concat(newTags).ToList();
                foreach (var tag in newTags)
                {
                    _context.Tags.Attach(tag); 
                }
            }
            _mapper.Map(eventDto, existingEvent);
            _context.Events.Update(existingEvent);  
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventDto>> SearchAsync(string keyword)
        {
            keyword = keyword?.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                return Enumerable.Empty<EventDto>();
            }
            var searchResults = await _context.Events
                .Include(e => e.Tags) 
                .Include(e => e.Comments) 
                .AsNoTracking()
                .Where(e => e.Title.Contains(keyword) || e.Desc.Contains(keyword) || e.Info.Contains(keyword) || e.Tags.Any(t => t.Name.Contains(keyword)))
                .ToListAsync();

            return _mapper.Map<IEnumerable<EventDto>>(searchResults);
        }


    }
}

