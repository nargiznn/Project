using System;
using AutoMapper;
using Domain.Entities;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.Subscribe;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class SubscribeService: ISubscribeService
    {
        private readonly ISubscribeRepository _subscribeRepository;
        private readonly IMapper _mapper;
        public SubscribeService(ISubscribeRepository subscribeRepository,
                                IMapper mapper)
        {
            _subscribeRepository = subscribeRepository;
            _mapper = mapper;
        }
        public async Task AddSubscribeAsync(SubscribeCreateDto subscribeCreateDto)
        {
            var IsExist = await _subscribeRepository.IsExist(m => m.Email.Trim().ToLower() == subscribeCreateDto.Email.Trim().ToLower());

            if (IsExist)
            {
                throw new BadRequestException("This email has already exist");
            }
            var subscribe = _mapper.Map<Subscribe>(subscribeCreateDto);
            await _subscribeRepository.CreateAsync(subscribe);
            await _subscribeRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<SubscribeDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<SubscribeDto>>(await _subscribeRepository.GetAllAsync());
        }
    }
}

