using System;
using AutoMapper;
using Repository.Data;
using Repository.Exceptions;
using System.Diagnostics.Metrics;
using Service.Services.Interfaces;
using Service.Helpers.DTOs.Chef;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Service.Helpers.DTOs.Event;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

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
            chef.Name = chef.Name?.Trim();
            chef.Surname = chef.Surname?.Trim();

            if (chef.SocialMedia != null)
            {
                chef.SocialMedia.FacebookUrl = chef.SocialMedia.FacebookUrl?.Trim();
                chef.SocialMedia.TwitterUrl = chef.SocialMedia.TwitterUrl?.Trim();
                chef.SocialMedia.InstagramUrl = chef.SocialMedia.InstagramUrl?.Trim();
            }

            if (chef.PositionIds != null && chef.PositionIds.Any())
            {
                chef.PositionIds = chef.PositionIds.Distinct().ToList();
                foreach (var positionId in chef.PositionIds)
                {
                    var positionExists = await _context.Positions.AnyAsync(p => p.Id == positionId);
                    if (!positionExists)
                    {
                        throw new NotFoundException($"Position ID {positionId} mövcud deyil.");
                    }
                }
            }
            var existingChef = await _context.Chefs
                .Include(x => x.ChefImages)
                .FirstOrDefaultAsync(x => x.Name == chef.Name && x.Surname == chef.Surname);

            if (existingChef != null && chef.Photos != null)
            {
                foreach (var photo in chef.Photos)
                {
                    if (await IsDuplicateImageAsync(photo, existingChef.ChefImages.ToList()))
                    {
                        throw new Exception("Bu adı və soyadı olan chef artıq eyni şəkil ilə mövcuddur.");
                    }
                }
            }

            var mappedData = _mapper.Map<Chef>(chef);
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
                    Path = $"http://localhost:7031/uploads/{response.Response}"
                });
            }
            foreach (var positionId in chef.PositionIds)
            {
                var existingPosition = await _context.ChefPositions
                                                     .FirstOrDefaultAsync(x => x.ChefId == mappedData.Id && x.PositionId == positionId);
                if (existingPosition == null) 
                {
                    await _context.ChefPositions.AddAsync(new ChefPosition
                    {
                        ChefId = mappedData.Id,
                        PositionId = positionId
                    });
                }
            }

            await _context.SaveChangesAsync();
        }


        private async Task<bool> IsDuplicateImageAsync(IFormFile photo, IEnumerable<ChefImage> existingImages)
        {
            using var sha256 = SHA256.Create();
            using var stream = photo.OpenReadStream();
            var hashBytes = await Task.Run(() => sha256.ComputeHash(stream));
            var newImageHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            foreach (var image in existingImages)
            {
                var existingImageHash = await CalculateFileHashAsync(image.Path);
                if (newImageHash == existingImageHash)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<string> CalculateFileHashAsync(string imagePath)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", Path.GetFileName(imagePath));
            if (!System.IO.File.Exists(filePath))
            {
                return string.Empty;
            }

            using var sha256 = SHA256.Create();
            await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var hashBytes = await sha256.ComputeHashAsync(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
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
            Chef existChef = await _context.Chefs
                .Include(x => x.SocialMedia)
                .Include(x => x.ChefImages)
                .Include(x => x.ChefPosition)
                .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException("Data not found!");
            existChef.Name = chef.Name?.Trim() ?? existChef.Name;
            existChef.Surname = chef.Surname?.Trim() ?? existChef.Surname;
            if (chef.SocialMedia != null)
            {
                existChef.SocialMedia.FacebookUrl = chef.SocialMedia.FacebookUrl?.Trim() ?? existChef.SocialMedia.FacebookUrl;
                existChef.SocialMedia.TwitterUrl = chef.SocialMedia.TwitterUrl?.Trim() ?? existChef.SocialMedia.TwitterUrl;
                existChef.SocialMedia.InstagramUrl = chef.SocialMedia.InstagramUrl?.Trim() ?? existChef.SocialMedia.InstagramUrl;
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
                        Path = $"http://localhost:7031/uploads/{response.Response}"
                    });
                }
            }
            if (chef.PositionIds != null && chef.PositionIds.Any())
            {
                _context.ChefPositions.RemoveRange(existChef.ChefPosition);
                foreach (var positionId in chef.PositionIds)
                {
                    var positionExists = await _context.Positions
                        .AnyAsync(p => p.Id == positionId);
                    if (!positionExists)
                    {
                        throw new NotFoundException($"Position ID {positionId} mövcud deyil.");
                    }
                    await _context.ChefPositions.AddAsync(new ChefPosition
                    {
                        ChefId = existChef.Id,
                        PositionId = positionId
                    });
                }
            }

            _context.Update(existChef);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existChef = await _context.Chefs.FirstOrDefaultAsync(x => x.Id == id)
                              ?? throw new NotFoundException("Chef not found");
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
        public async Task<IEnumerable<ChefDto>> SearchAsync(string keyword)
        {
            keyword = keyword?.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                return Enumerable.Empty<ChefDto>();
            }

            var searchResults = await _context.Chefs
                .Include(e => e.SocialMedia) 
                .Include(e => e.ChefPosition) 
                    .ThenInclude(p => p.Position)
                .AsNoTracking()
                .Where(e => e.Name.Contains(keyword) || e.Surname.Contains(keyword) 
                        || (e.SocialMedia != null &&
                            (e.SocialMedia.FacebookUrl.Contains(keyword) ||
                             e.SocialMedia.TwitterUrl.Contains(keyword) ||
                             e.SocialMedia.InstagramUrl.Contains(keyword))) 
                        || e.ChefPosition.Any(cp => cp.Position.Title.Contains(keyword)))
                .ToListAsync();

            return _mapper.Map<IEnumerable<ChefDto>>(searchResults);
        }

    }
}

