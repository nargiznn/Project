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
            if (string.IsNullOrWhiteSpace(award.Year) || !int.TryParse(award.Year, out int year))
            {
                throw new ArgumentException("Invalid year format.");
            }
            if (year > DateTime.Now.Year)
            {
                throw new ArgumentException("You cannot set an award for a future year.");
            }

            DateTime awardYear = new DateTime(year, 1, 1);
            award.Name = award.Name?.Trim();

            var existingAward = await _awardRepo.GetAllWithExpression(
                x => x.Name == award.Name && x.Year.Year == awardYear.Year
            );
            if (existingAward.Any())
            {
                throw new ArgumentException("An award with the same name and year already exists.");
            }

            var awardEntity = _mapper.Map<Award>(award);
            awardEntity.Year = awardYear; 

            await _awardRepo.CreateAsync(awardEntity);
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

            DateTime newAwardYear;
            if (award.Year != null && int.TryParse(award.Year, out int year))
            {
                if (year > DateTime.Now.Year)
                {
                    throw new ArgumentException("You cannot set an award for a future year.");
                }

                newAwardYear = new DateTime(year, 1, 1);
            }
            else
            {
                newAwardYear = existingAward.Year; 
            }
            award.Name = award.Name?.Trim();

            var duplicateAward = await _awardRepo.GetAllWithExpression(
                x => x.Name == (award.Name ?? existingAward.Name) &&
                     x.Year.Year == newAwardYear.Year &&
                     x.Id != id
            );

            if (duplicateAward.Any())
            {
                throw new ArgumentException("An award with the same name and year already exists.");
            }
            existingAward.Name = string.IsNullOrWhiteSpace(award.Name) ? existingAward.Name : award.Name;
            existingAward.Year = newAwardYear;

            await _awardRepo.EditAsync(existingAward);
        }





    }
}

