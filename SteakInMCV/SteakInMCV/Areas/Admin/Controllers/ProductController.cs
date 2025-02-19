using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.AwardLogo;
using SteakInMCV.Areas.Admin.ViewModels.Cuisine;
using SteakInMCV.Areas.Admin.ViewModels.MenuCategory;
using SteakInMCV.Areas.Admin.ViewModels.Product;
using SteakInMCV.Areas.Admin.ViewModels.SpecialCategory;
using SteakInMCV.Models;

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";

        public async Task<IActionResult> Index(int page = 1, int size = 10)
        {
            IEnumerable<ProductVM> productList = null;

            using (var httpClient = new HttpClient())
            {
                var url = $"{BaseURl}/api/Product/GetAll?page={page}&size={size}";
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    productList = JsonConvert.DeserializeObject<List<ProductVM>>(apiResponse);
                }
            }
            var totalPages = (int)Math.Ceiling(productList.Count() / (double)size);
            var paginatedList = new PaginatedList<ProductVM>(
                productList.Skip((page - 1) * size).Take(size).ToList(),
                totalPages,
                page,
                size
            );

            return View(paginatedList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"{BaseURl}/api/product/delete/" + id);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "An error occurred while deleting the item." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ProductVM product = null;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/product/getbyid/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<ProductVM>(apiResponse);
                }
            }

            await LoadSelectLists();

            var menuCategoryId = (await GetAllMenuCategoriesAsync())
                                    .FirstOrDefault(m => m.Name == product.MenuCategoryName)?.Id;
            var specialCategoryId = (await GetAllSpecialCategoriesAsync())
                                    .FirstOrDefault(s => s.Name == product.SpecialCategoryName)?.Id;
            var productCuisineId = (await GetAllCuisinesAsync())
                                    .FirstOrDefault(c => c.Name == product.ProductCuisineName)?.Id;

            return View(new ProductEditVM
            {
                Id = product.Id,
                Name = product.Name,
                Ingredient = product.Ingredient,
                Price = product.Price,
                MenuCategoryId = menuCategoryId ?? 0,
                SpecialCategoryId = specialCategoryId ?? 0,
                ProductCuisineId = productCuisineId ?? 0
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            ProductVM currentProduct = await GetProductByIdAsync(id);

            if (request.Price <= 0)
            {
                ModelState.AddModelError(nameof(request.Price), "Price must be greater than zero.");
            }

            request.Name = request.Name ?? currentProduct.Name;
            request.Ingredient = request.Ingredient ?? currentProduct.Ingredient;
            request.Price = request.Price > 0 ? request.Price : currentProduct.Price;
            request.MenuCategoryId = request.MenuCategoryId ?? currentProduct.MenuCategoryId;
            request.SpecialCategoryId = request.SpecialCategoryId ?? currentProduct.SpecialCategoryId;
            request.ProductCuisineId = request.ProductCuisineId ?? currentProduct.ProductCuisineId;

            using (var httpClient = new HttpClient())
            {
                using (var multipartContent = new MultipartFormDataContent())
                {
                    multipartContent.Add(new StringContent(request.Name), "Name");
                    multipartContent.Add(new StringContent(request.Ingredient), "Ingredient");
                    multipartContent.Add(new StringContent(request.Price.ToString()), "Price");
                    multipartContent.Add(new StringContent(request.MenuCategoryId.ToString()), "MenuCategoryId");
                    multipartContent.Add(new StringContent(request.SpecialCategoryId?.ToString() ?? string.Empty), "SpecialCategoryId");
                    multipartContent.Add(new StringContent(request.ProductCuisineId.ToString()), "CuisineId");

                    if (request.Files != null && request.Files.Any())
                    {
                        foreach (var file in request.Files)
                        {
                            var fileContent = new StreamContent(file.OpenReadStream());
                            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                            multipartContent.Add(fileContent, "Files", file.FileName);
                        }
                    }

                    using (var response = await httpClient.PutAsync($"{BaseURl}/api/product/edit/{id}", multipartContent))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                        {
                            ModelState.AddModelError(string.Empty, "API-də xəta baş verdi.");
                            await LoadSelectLists();
                            return View(request);
                        }
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProductVM> GetProductByIdAsync(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{BaseURl}/api/product/getbyid/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ProductVM>(apiResponse);
                }
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            ProductVM awardLogo = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/product/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    awardLogo = JsonConvert.DeserializeObject<ProductVM>(apiResponse);
                }
            }

            return View(awardLogo);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadSelectLists();
            return View();
        }

        private async Task LoadSelectLists()
        {
            using var httpClient = new HttpClient();

            var menuCategoryResponse = await httpClient.GetStringAsync($"{BaseURl}/api/MenuCategory/GetAll");
            ViewBag.MenuCategories = JsonConvert.DeserializeObject<IEnumerable<MenuCategoryVM>>(menuCategoryResponse);

            var specialCategoryResponse = await httpClient.GetStringAsync($"{BaseURl}/api/SpecialCategory/GetAll");
            ViewBag.SpecialCategories = JsonConvert.DeserializeObject<IEnumerable<SpecialCategoryVM>>(specialCategoryResponse);

            var cuisineResponse = await httpClient.GetStringAsync($"{BaseURl}/api/Cuisine/GetAll");
            ViewBag.Cuisines = JsonConvert.DeserializeObject<IEnumerable<CuisineVM>>(cuisineResponse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            if (request.Price <= 0)
            {
                ModelState.AddModelError(nameof(request.Price), "The price must be greater than zero.");
            }

            var existingProducts = await GetAllProductsAsync();
            if (existingProducts.Any(p => p.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(nameof(request.Name), "A product with this name already exists.");
            }

            var specialCategories = await GetAllSpecialCategoriesAsync();
            if (!specialCategories.Any(s => s.Id == request.SpecialCategoryId))
            {
                ModelState.AddModelError(nameof(request.SpecialCategoryId), "Invalid Special Category.");
            }

            var menuCategories = await GetAllMenuCategoriesAsync();
            if (!menuCategories.Any(m => m.Id == request.MenuCategoryId))
            {
                ModelState.AddModelError(nameof(request.MenuCategoryId), "Invalid Menu Category.");
            }

            var cuisines = await GetAllCuisinesAsync();
            if (!cuisines.Any(c => c.Id == request.CuisineId))
            {
                ModelState.AddModelError(nameof(request.CuisineId), "Invalid Cuisine.");
            }

            if (!ModelState.IsValid)
            {
                await LoadSelectLists();
                return View(request);
            }

            using (var httpClient = new HttpClient())
            {
                using (var multipartContent = new MultipartFormDataContent())
                {
                    multipartContent.Add(new StringContent(request.Name), "Name");
                    multipartContent.Add(new StringContent(request.Ingredient), "Ingredient");
                    multipartContent.Add(new StringContent(request.Price.ToString()), "Price");
                    multipartContent.Add(new StringContent(request.MenuCategoryId.ToString()), "MenuCategoryId");
                    multipartContent.Add(new StringContent(request.SpecialCategoryId?.ToString() ?? string.Empty), "SpecialCategoryId");
                    multipartContent.Add(new StringContent(request.CuisineId.ToString()), "CuisineId");

                    if (request.Files != null && request.Files.Any())
                    {
                        foreach (var file in request.Files)
                        {
                            var fileContent = new StreamContent(file.OpenReadStream());
                            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                            multipartContent.Add(fileContent, "Files", file.FileName);
                        }
                    }

                    using (var response = await httpClient.PostAsync($"{BaseURl}/api/product/create", multipartContent))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                        {
                            ModelState.AddModelError(string.Empty, "API-də xəta baş verdi.");
                            await LoadSelectLists();
                            return View(request);
                        }
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<MenuCategoryVM>> GetAllMenuCategoriesAsync()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync($"{BaseURl}/api/MenuCategory/GetAll");
            return JsonConvert.DeserializeObject<IEnumerable<MenuCategoryVM>>(response);
        }

        private async Task<IEnumerable<SpecialCategoryVM>> GetAllSpecialCategoriesAsync()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync($"{BaseURl}/api/SpecialCategory/GetAll");
            return JsonConvert.DeserializeObject<IEnumerable<SpecialCategoryVM>>(response);
        }

        private async Task<IEnumerable<CuisineVM>> GetAllCuisinesAsync()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync($"{BaseURl}/api/Cuisine/GetAll");
            return JsonConvert.DeserializeObject<IEnumerable<CuisineVM>>(response);
        }

        private async Task<IEnumerable<ProductVM>> GetAllProductsAsync()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync($"{BaseURl}/api/Product/GetAll");
            return JsonConvert.DeserializeObject<IEnumerable<ProductVM>>(response);
        }
    }
}
