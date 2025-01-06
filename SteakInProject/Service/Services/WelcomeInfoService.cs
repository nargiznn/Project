using System;
using AutoMapper;
using Domain.Entities;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.WelcomeInfo;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class WelcomeInfoService: IWelcomeInfoService
    {
        private readonly IWelcomeInfoRepository _welcomeInfoRepo;
        private readonly IMapper _mapper;
        public WelcomeInfoService(IWelcomeInfoRepository welcomeInfoRepository, IMapper mapper)
        {
            _welcomeInfoRepo = welcomeInfoRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(WelcomeInfoCreateDto welcomeInfo)
        {
            await _welcomeInfoRepo.CreateAsync(_mapper.Map<WelcomeInfo>(welcomeInfo));
        }

        public async Task DeleteAsync(int id)
        {
            await _welcomeInfoRepo.DeleteAsync(id);
        }

        public async Task<IEnumerable<WelcomeInfoDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<WelcomeInfoDto>>(await _welcomeInfoRepo.GetAllAsync());
        }

        public async Task<WelcomeInfoDto> GetByIdAsync(int id)
        {
            return _mapper.Map<WelcomeInfoDto>(await _welcomeInfoRepo.GetByIdAsync(id));
        }
        public async Task EditAsync(int id, WelcomeInfoEditDto welcomeInfo)
        {
            var existingWelcomeInfo = await _welcomeInfoRepo.GetByIdAsync(id);
            if (existingWelcomeInfo == null)
            {
                throw new NotFoundException("WelcomeInfo not found");
            }
            if (!string.IsNullOrEmpty(welcomeInfo.Title))
            {
                existingWelcomeInfo.Title = welcomeInfo.Title;
            }
            if (!string.IsNullOrEmpty(welcomeInfo.MainTitle))
            {
                existingWelcomeInfo.MainTitle = welcomeInfo.MainTitle;
            }
            if (!string.IsNullOrEmpty(welcomeInfo.Icon))
            {
                existingWelcomeInfo.Icon = welcomeInfo.Icon;
            }
            if (!string.IsNullOrEmpty(welcomeInfo.Desc))
            {
                existingWelcomeInfo.Desc = welcomeInfo.Desc;
            }
            if (!string.IsNullOrEmpty(welcomeInfo.BtnText))
            {
                existingWelcomeInfo.BtnText = welcomeInfo.BtnText;
            }
            await _welcomeInfoRepo.EditAsync(existingWelcomeInfo);
        }

    }
}

