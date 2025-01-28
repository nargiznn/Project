using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.AwardLogo;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class AwardLogoService:IAwardLogoService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public AwardLogoService(AppDbContext ccontext,
                           IMapper mapper,
                           IFileService fileService)
        {
            _context = ccontext;
            _mapper = mapper;
            _fileService = fileService;
        }
        public async Task CreateAsync(AwardLogoCreateDto awardLogo)
        {
            var response = await _fileService.UploadAsync(awardLogo.Image);


            var mappedData = _mapper.Map<AwardLogo>(awardLogo);
            mappedData.Image = $"http://localhost:7031/uploads/{response.Response}";

            await _context.AwardLogos.AddAsync(mappedData);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var awardLogo = await _context.AwardLogos.AsNoTracking()
                                               .FirstOrDefaultAsync(x => x.Id == id)
                                                   ?? throw new NotFoundException("AwardLogo not found");
            _context.AwardLogos.Remove(awardLogo);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, AwardLogoEditDto awardLogo)
        {
            var existAwardLogo = await _context.AwardLogos.FirstOrDefaultAsync(x => x.Id == id)
                                      ?? throw new NotFoundException("AwardLogo not found");

            if (awardLogo.Image != null)
            {
                var response = await _fileService.UploadAsync(awardLogo.Image);
                existAwardLogo.Image = $"http://localhost:7031/uploads/{response.Response}";
            }

            _mapper.Map(awardLogo, existAwardLogo);

            _context.Update(existAwardLogo);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<AwardLogoDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<AwardLogoDto>>(await _context.AwardLogos.AsNoTracking()
                                                                             .ToListAsync());
        }

        public async Task<AwardLogoDto> GetByIdAsync(int id)
        {
            return _mapper.Map<AwardLogoDto>(await _context.AwardLogos.AsNoTracking()
                                                                .FirstOrDefaultAsync(x => x.Id == id))
                                                                    ?? throw new NotFoundException("AwardLogo not found");
        }
    }
}

