using System;
using System.Xml.Linq;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Data;
using Service.Helpers.DTOs.Event;
using Service.Helpers.DTOs.Product;
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
                        Path = Path.Combine("uploads", fileResponse.Response)
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
                FoodCategoryId = product.FoodCategoryId,
                CuisineId = product.CuisineId,
                ProductImages = productImages
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return "Uğurla əlavə edildi";
        }




        public async Task<string> DeleteAsync(int id)
        {
            var findData = await _context.Products.FindAsync(id);
            if (findData == null)
            {
                return "Data not found";
            }
            foreach (var image in findData.ProductImages)
            {
                await _fileService.DeletePath(image.Image); 
            }

            _context.Products.Remove(findData);
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

            if (!string.IsNullOrEmpty(product.Name)) findProduct.Name = product.Name;
            if (!string.IsNullOrEmpty(product.Ingredient)) findProduct.Ingredient = product.Ingredient;
            if (product.Price.HasValue) findProduct.Price = product.Price.Value;
            if (product.MenuCategoryId.HasValue) findProduct.MenuCategoryId = product.MenuCategoryId.Value;
            if (product.SpecialCategoryId.HasValue) findProduct.SpecialCategoryId = product.SpecialCategoryId.Value;
            if (product.FoodCategoryId.HasValue) findProduct.FoodCategoryId = product.FoodCategoryId.Value;
            if (product.ProductCuisineId.HasValue) findProduct.CuisineId = product.ProductCuisineId.Value;


            if (product.Files != null && product.Files.Count > 0)
            {
                foreach (var oldImage in findProduct.ProductImages)
                {
                    await _fileService.DeletePath(oldImage.Image);
                }

                var newImages = new List<ProductImage>();
                foreach (var file in product.Files)
                {
                    var fileResponse = await _fileService.UploadAsync(file);
                    if (fileResponse.HasError)
                    {
                        return fileResponse.Response;
                    }

                    newImages.Add(new ProductImage
                    {
                        Image = fileResponse.Response,
                        Path = Path.Combine("uploads", fileResponse.Response)
                    });
                }

                findProduct.ProductImages = newImages;
            }

            await _context.SaveChangesAsync();
            return "Success";
        }


        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _context.Products
                                        .Include(e => e.Cuisine)
                                        .Include(e=>e.MenuCategory)
                                        .Include(e => e.SpecialCategory)
                                        .Include(e => e.FoodCategory)
                                        .Include(e => e.ProductImages)
                                        .AsNoTracking()
                                        .ToListAsync();

            return _mapper.Map<List<ProductDto>>(products);
        }

        public Task<Product> GetById(int id)
        {
            return _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

