using System;
using System.Reflection.Metadata;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.Event;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class EventService : IEventService
	{
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public EventService(IEventRepository eventRepository, IMapper mapper, IFileService fileService)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task CreateAsync(EventCreateDto events)
        {
            if (events is null) throw new ArgumentNullException(nameof(events));

            var eventEntity = _mapper.Map<Event>(events);

            //if (events.TagIds?.Any() == true)
            //{
            //    eventEntity.Tags = await _eventRepository.GetAllWithExpression(t => events.TagIds.Contains(t.Id)).ToListAsync();
            //}

            if (events.UploadImage != null)
            {
                var uploadedFile = await _fileService.UploadAsync(events.UploadImage);
                eventEntity.ImgUrl = uploadedFile.Response;
            }

            await _eventRepository.CreateAsync(eventEntity);
        }

        public async Task EditAsync(int id, EventEditDto events)
        {
            var existingEvent = await _eventRepository.GetByIdAsync(id);
            if (existingEvent is null) throw new NotFoundException("Event not found");

            _mapper.Map(events, existingEvent);

            //if (events.TagIds?.Any() == true)
            //{
            //    existingEvent.Tags = await _eventRepository.GetAllWithExpression(t => events.TagIds.Contains(t.Id)).ToListAsync();
            //}

            if (events.UploadImage != null)
            {
                _fileService.DeletePath(existingEvent.ImgUrl);
                var uploadedFile = await _fileService.UploadAsync(events.UploadImage);
                existingEvent.ImgUrl = uploadedFile.Response;
            }

            await _eventRepository.EditAsync(existingEvent);
        }

        public async Task<IEnumerable<EventDto>> GetAllAsync()
        {
            var events = await _eventRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task<EventDto> GetByIdAsync(int id)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(id);
            return _mapper.Map<EventDto>(eventEntity);
        }

        public async Task DeleteAsync(int id)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(id);
            _fileService.DeletePath(eventEntity.ImgUrl);
            await _eventRepository.DeleteAsync(id);
        }
    }
}

