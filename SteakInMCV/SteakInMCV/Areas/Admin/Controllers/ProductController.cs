using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.AwardLogo;
using SteakInMCV.Areas.Admin.ViewModels.Cuisine;
using SteakInMCV.Areas.Admin.ViewModels.MenuCategory;
using SteakInMCV.Areas.Admin.ViewModels.Product;
using SteakInMCV.Areas.Admin.ViewModels.SpecialCategory;

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController:Controller
	{
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductVM> awardLogo = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/Product/GetAll"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    awardLogo = JsonConvert.DeserializeObject<IEnumerable<ProductVM>>(apiResponse);
                }
            }
            return View(awardLogo);
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
                return Json(new { success = false, message = "Məhsul silinərkən xəta baş verdi." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ProductVM product = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/product/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<ProductVM>(apiResponse);
                }
            }
            return View(new ProductEditVM
            {
                Id = product.Id,
                Name = product.Name,
                Ingredient = product.Ingredient,
                Price = product.Price,
                MenuCategoryId=product.MenuCategoryId,
                SpecialCategoryId=product.SpecialCategoryId,
                ProductCuisineId=product.ProductCuisineId
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
                        foreach (var photo in request.Files)
                        {
                            var fileContent = new StreamContent(photo.OpenReadStream());
                            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(photo.ContentType);
                            multipartContent.Add(fileContent, "Photos", photo.FileName);
                        }
                    }

                    using (var response = await httpClient.PutAsync($"{BaseURl}/api/product/edit/{id}", multipartContent))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                        {
                            ModelState.AddModelError(string.Empty, "API-də xəta baş verdi.");
                            return View(request);
                        }
                    }
                }
            }

            return RedirectToAction(nameof(Index));

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

    }
}

