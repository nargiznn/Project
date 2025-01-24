using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.Award;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class AwardService:IAwardService
	{
        private readonly IAwardRepository _awardRepo;
        private readonly IMapper _mapper;
        public AwardService(IAwardRepository awardRepository, IMapper mapper)
        {
            _awardRepo = awardRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(AwardCreateDto award)
        {
            if (award.Year > DateTime.Now)
            {
                throw new ArgumentException("The year cannot be in the future.");
            }

            var existingAward = await _awardRepo.GetAllWithExpression(
                x => x.Name == award.Name && x.Year.Year == award.Year.Year
            );
            if (existingAward.Any())
            {
                throw new ArgumentException("An award with the same name and year already exists.");
            }

            await _awardRepo.CreateAsync(_mapper.Map<Award>(award));
        }

        public async Task DeleteAsync(int id)
        {
            await _awardRepo.DeleteAsync(id);
        }

        public async Task<IEnumerable<AwardDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<AwardDto>>(await _awardRepo.GetAllAsync());
        }

        public async Task<AwardDto> GetByIdAsync(int id)
        {
            return _mapper.Map<AwardDto>(await _awardRepo.GetByIdAsync(id));
        }

        public async Task<IEnumerable<AwardDto>> SearchAsync(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                var allAwards = await _awardRepo.GetAllAsync();
                return _mapper.Map<IEnumerable<AwardDto>>(allAwards);
            }
            var awards = await _awardRepo.GetAllWithExpression(c =>
                c.Name.Contains(str) || c.Year.Year.ToString() == str 
            );

            if (!awards.Any()) 
            {
                throw new NotFoundException("No awards found matching the search criteria.");
            }

            return _mapper.Map<IEnumerable<AwardDto>>(awards);
        }


        public async Task EditAsync(int id, AwardEditDto award)
        {
            var existingAward = await _awardRepo.GetByIdAsync(id);
            if (existingAward == null)
            {
                throw new NotFoundException("Award not found");
            }
            if (award.Year.HasValue && award.Year.Value > DateTime.Now)
            {
                throw new ArgumentException("The year cannot be in the future.");
            }

            var duplicateAward = await _awardRepo.GetAllWithExpression(
                x => x.Name == (award.Name ?? existingAward.Name) &&
                     x.Year.Year == ((award.Year.HasValue ? award.Year.Value.Year : existingAward.Year.Year)) &&
                     x.Id != id 
            );


            if (duplicateAward.Any())
            {
                throw new ArgumentException("An award with the same name and year already exists.");
            }

            existingAward.Name = string.IsNullOrWhiteSpace(award.Name) ? existingAward.Name : award.Name;
            existingAward.Year = award.Year ?? existingAward.Year;

            await _awardRepo.EditAsync(existingAward);
        }
    }
}

