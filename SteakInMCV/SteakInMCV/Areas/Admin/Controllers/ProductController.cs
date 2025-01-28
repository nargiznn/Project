using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{BaseURl}/api/product/delete/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ProductEditVM viewModel = new ProductEditVM();

            using (var httpClient = new HttpClient())
            {
                // MenuCategories
                var menuCategoryResponse = await httpClient.GetStringAsync($"{BaseURl}/api/menucategory/GetAll");
                var menuCategories = JsonConvert.DeserializeObject<List<MenuCategoryVM>>(menuCategoryResponse);
                viewModel.MenuCategories = menuCategories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == viewModel.MenuCategoryId // Seçili etmək
                }).ToList();

                // SpecialCategories
                var specialCategoryResponse = await httpClient.GetStringAsync($"{BaseURl}/api/specialcategory/GetAll");
                var specialCategories = JsonConvert.DeserializeObject<List<SpecialCategoryVM>>(specialCategoryResponse);
                viewModel.SpecialCategories = specialCategories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == viewModel.SpecialCategoryId // Seçili etmək
                }).ToList();

                // Cuisines
                var cuisineResponse = await httpClient.GetStringAsync($"{BaseURl}/api/cuisine/GetAll");
                var cuisines = JsonConvert.DeserializeObject<List<CuisineVM>>(cuisineResponse);
                viewModel.ProductCuisines = cuisines.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == viewModel.ProductCuisineId // Seçili etmək
                }).ToList();

                // Product
                var productResponse = await httpClient.GetStringAsync($"{BaseURl}/api/Product/GetById/" + id);
                var product = JsonConvert.DeserializeObject<ProductVM>(productResponse);

                viewModel.Name = product.Name;
                viewModel.Ingredient = product.Ingredient;
                viewModel.Price = (decimal)product.Price;
                viewModel.MenuCategoryId = product.MenuCategoryId;
                viewModel.SpecialCategoryId = product.SpecialCategoryId;
                viewModel.ProductCuisineId = product.CuisineId;  // Dəyişiklik edilən məhsulun seçili id-ləri
            }

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditVM request)
        {
            if (!ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync($"{BaseURl}/api/menucategory/GetAll");
                    var subCategories = JsonConvert.DeserializeObject<List<MenuCategoryVM>>(response);
                    request.MenuCategories = subCategories.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();
                }

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync($"{BaseURl}/api/specialcategory/GetAll");
                    var brands = JsonConvert.DeserializeObject<List<SpecialCategoryVM>>(response);

                    request.SpecialCategories = brands.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();
                }


                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync($"{BaseURl}/api/cuisine/GetAll");
                    var categories = JsonConvert.DeserializeObject<List<CuisineVM>>(response);

                    request.ProductCuisines = categories.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();
                }
                return View(request);
            }

            using (var httpClient = new HttpClient())
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(request.Name), "Name");
                formData.Add(new StringContent(request.Price.ToString()), "Price");
                formData.Add(new StringContent(request.Ingredient), "Ingredient");
                formData.Add(new StringContent(request.MenuCategoryId.ToString()), "MenuCategoryId");
                formData.Add(new StringContent(request.SpecialCategoryId.ToString()), "SpecialCategoryId");
                formData.Add(new StringContent(request.ProductCuisineId.ToString()), "ProductCuisineId");


                if (request.Files != null)
                {
                    foreach (var file in request.Files)
                    {
                        if (file.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await file.CopyToAsync(memoryStream);
                                var fileContent = new ByteArrayContent(memoryStream.ToArray());
                                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
                                formData.Add(fileContent, "UploadImages", file.FileName);
                            }
                        }
                    }

                }

                using (var response = await httpClient.PutAsync($"{BaseURl}/api/Product/Edit/{id}", formData))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }


    }
}

