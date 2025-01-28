using System;
using System.Xml.Linq;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Data;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.Event;
using Service.Helpers.DTOs.Product;
using Service.Helpers.Faqs;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class ProductService:IProductService
	{
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext context,
                             IFileService fileService,
                             IMapper mapper)
        {
            _context = context;
            _fileService = fileService;
            _mapper = mapper;
        }
        public async Task<string> CreateAsync(ProductCreateDto product)
        {
            if (product.Price <= 0)
            {
                return "Price must be greater than zero.";
            }
            var productImages = new List<ProductImage>();
            if (product.Files != null && product.Files.Count > 0)
            {
                foreach (var file in product.Files)
                {
                    var fileResponse = await _fileService.UploadAsync(file);
                    if (fileResponse.HasError)
                    {
                        return fileResponse.Response;
                    }

                    productImages.Add(new ProductImage
                    {
                        Image = fileResponse.Response,
                        Path = Path.Combine("http://localhost:7031/uploads/", fileResponse.Response)
                    });
                }
            }

            var newProduct = new Product
            {
                Name = product.Name,
                Ingredient = product.Ingredient,
                Price = product.Price,
                MenuCategoryId = product.MenuCategoryId,
                SpecialCategoryId = product.SpecialCategoryId,
                CuisineId = product.CuisineId,
                ProductImages = productImages
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return "Success"; 
        }





        public async Task<string> DeleteAsync(int id)
        {
            var exisProduct = await _context.Products.FindAsync(id);
            if (exisProduct == null)
            {
                return "Data not found";
            }
            _context.Products.Remove(exisProduct);
            foreach (var item in _context.ProductImages.Where(x => x.ProductId == exisProduct.Id))
            {
                _fileService.DeletePath(item.Image);
                _context.ProductImages.Remove(item);
            }

            await _context.SaveChangesAsync();

            return "Success";
        }



        public async Task<string> EditAsync(int id, ProductEditDto product)
        {
            var findProduct = await _context.Products
                                            .Include(p => p.ProductImages)
                                            .FirstOrDefaultAsync(p => p.Id == id);

            if (findProduct == null)
            {
                return "Data not found";
            }
            if (product.Price.HasValue)
            {
                if (product.Price.Value <= 0)
                {
                    return "Price must be greater than zero.";
                }
                findProduct.Price = product.Price.Value;
            }
            if (!string.IsNullOrEmpty(product.Name)) findProduct.Name = product.Name;
            if (!string.IsNullOrEmpty(product.Ingredient)) findProduct.Ingredient = product.Ingredient;
            if (product.Price.HasValue)
            {
                if (product.Price.Value > 0 && findProduct.Price != product.Price.Value)
                {
                    findProduct.Price = product.Price.Value;
                }
            }

            if (product.MenuCategoryId.HasValue) findProduct.MenuCategoryId = product.MenuCategoryId.Value;
            if (product.SpecialCategoryId.HasValue) findProduct.SpecialCategoryId = product.SpecialCategoryId.Value;
            if (product.ProductCuisineId.HasValue) findProduct.CuisineId = product.ProductCuisineId.Value;
            if (product.Files != null && product.Files.Count > 0)
            {
                foreach (var oldImage in findProduct.ProductImages)
                {
                    _fileService.DeletePath(oldImage.Image);
                }
                var newImages = new List<ProductImage>();
                foreach (var file in product.Files)
                {
                    var response = await _fileService.UploadAsync(file);
                    if (response.HasError)
                    {
                        throw new BadHttpRequestException(response.Response);
                    }

                    newImages.Add(new ProductImage
                    {
                        ProductId = findProduct.Id,
                        Image = response.Response,
                        Path = $"http://localhost:7031/uploads/{response.Response}"
                    });
                }
                findProduct.ProductImages.Clear();
                findProduct.ProductImages = newImages;
            }

            await _context.SaveChangesAsync();
            return "Success";
        }



        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _context.Products
                                        .Include(e => e.Cuisine)
                                        .Include(e => e.MenuCategory)
                                        .Include(e => e.SpecialCategory)
                                        .Include(e => e.ProductImages)
                                        .AsNoTracking()
                                        .ToListAsync();

            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var result = await _context.Products
                              .Include(p => p.MenuCategory)
                              .Include(p => p.SpecialCategory)
                              .Include(p => p.Cuisine)
                              .Include(p => p.ProductImages)
                              .AsNoTracking()
                              .FirstOrDefaultAsync(m => m.Id == id);

            if (result is null) return null;

            return _mapper.Map<ProductDto>(result);
        }
    }
}

