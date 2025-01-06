using System;
using AutoMapper;
using Repository.Data;
using Repository.Exceptions;
using System.Diagnostics.Metrics;
using Service.Services.Interfaces;
using Service.Helpers.DTOs.Chef;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Service.Services
{
	public class ChefService:IChefService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public ChefService(AppDbContext ccontext,
                           IMapper mapper,
                           IFileService fileService)
        {
            _context = ccontext;
            _mapper = mapper;
            _fileService = fileService;
        }


        public async Task CreateAsync(ChefCreateDto chef)
        {
            var mappedData = _mapper.Map<Chef>(chef);
            if (chef.SocialMedia != null)
            {
                var socialMedia = new SocialMediaLink
                {
                    FacebookUrl = chef.SocialMedia.FacebookUrl,
                    TwitterUrl = chef.SocialMedia.TwitterUrl,
                    InstagramUrl = chef.SocialMedia.InstagramUrl
                };
                await _context.SocialMediaLinks.AddAsync(socialMedia);
                await _context.SaveChangesAsync();
                mappedData.SocialMedia = socialMedia;
            }
            await _context.Chefs.AddAsync(mappedData);
            await _context.SaveChangesAsync();
            foreach (var item in chef.Photos)
            {
                var response = await _fileService.UploadAsync(item);
                if (response.HasError)
                {
                    throw new Exception("Fotoşəkil yükləmə xətası");
                }

                await _context.ChefImages.AddAsync(new ChefImage
                {
                    ChefId = mappedData.Id,
                    Image = $"{response.Response}",
                    Path = $"https://localhost:7031/uploads/{response.Response}"
                });
            }

            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<ChefDto>> GetAllAsync()
        {
            var chefs = await _context.Chefs
                    .Include(x => x.ChefImages) 
                    .Include(x => x.ChefPosition) 
                        .ThenInclude(x => x.Position) 
                    .Include(x => x.SocialMedia)
                    .AsNoTracking() 
                    .ToListAsync();

            return _mapper.Map<IEnumerable<ChefDto>>(chefs);
        }

        public async Task<ChefDto> GetByIdAsync(int id)
        {
            var chef = await _context.Chefs
                .Include(x => x.ChefImages) 
                .Include(x => x.ChefPosition) 
                    .ThenInclude(x => x.Position)
                .Include(x => x.SocialMedia) 
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (chef == null)
                throw new NotFoundException("Chef tapılmadı");

            return _mapper.Map<ChefDto>(chef);
        }
        public async Task EditAsync(int id, ChefEditDto chef)
        {
            // Mövcud chef tapılır
            Chef existChef = await _context.Chefs
                .Include(x => x.SocialMedia)
                .Include(x => x.ChefImages) 
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new NotFoundException("Data not found!");
            existChef.Name = chef.Name ?? existChef.Name;
            existChef.Surname = chef.Surname ?? existChef.Surname;

            if (chef.SocialMedia != null)
            {
                existChef.SocialMedia.FacebookUrl = chef.SocialMedia.FacebookUrl ?? existChef.SocialMedia.FacebookUrl;
                existChef.SocialMedia.TwitterUrl = chef.SocialMedia.TwitterUrl ?? existChef.SocialMedia.TwitterUrl;
                existChef.SocialMedia.InstagramUrl = chef.SocialMedia.InstagramUrl ?? existChef.SocialMedia.InstagramUrl;
            }
            if (chef.Photos != null && chef.Photos.Any())
            {
                foreach (var image in existChef.ChefImages)
                {
                    _fileService.DeletePath(image.Image);
                    _context.ChefImages.Remove(image); 
                }
                foreach (var photo in chef.Photos)
                {
                    var response = await _fileService.UploadAsync(photo);
                    if (response.HasError)
                    {
                        throw new Exception("Şəkil yükləmə xətası");
                    }

                    await _context.ChefImages.AddAsync(new ChefImage
                    {
                        ChefId = existChef.Id,
                        Image = $"{response.Response}",
                        Path = $"https://localhost:7031/uploads/{response.Response}"
                    });
                }
            }

            _context.Update(existChef);
            await _context.SaveChangesAsync();
        }



        public async Task DeleteAsync(int id)
        {
            var existChef = await _context.Chefs.FindAsync(id);
            _context.Chefs.Remove(existChef);
            foreach (var item in _context.ChefImages.Where(x => x.ChefId == existChef.Id))
            {
                _fileService.DeletePath(item.Image);
                _context.ChefImages.Remove(item);
            }

            foreach (var item in _context.ChefPositions.Where(x => x.ChefId == existChef.Id))
            {
                _context.ChefPositions.Remove(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddPosition(int chefId, int positionId)
        {
            var existChef = await _context.Chefs.FindAsync(chefId)
                                                        ?? throw new NotFoundException("Chef not found");
            var existPosition = await _context.Positions.FindAsync(positionId)
                                                            ?? throw new NotFoundException("Position not found");
            await _context.ChefPositions.AddAsync(new ChefPosition { PositionId = positionId, ChefId = chefId });
            await _context.SaveChangesAsync();
        }
    }
}

