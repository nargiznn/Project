using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.Chef;
using SteakInMCV.Areas.Admin.ViewModels.Position;
using SteakInMCV.Areas.Admin.ViewModels.SocialMediaLink;
using SteakInMCV.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChefController : Controller
    {
        private readonly string BaseURL = "http://localhost:7031";
        private readonly HttpClient _httpClient;

        public ChefController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        private async Task<T> GetApiResponse<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{BaseURL}/{endpoint}");
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(apiResponse);
        }
        public async Task<IActionResult> Index(string searchTerm)
        {
            IEnumerable<ChefVM> chefs = await GetApiResponse<IEnumerable<ChefVM>>("api/Chef/GetAll");
            if (!string.IsNullOrEmpty(searchTerm))
            {
                chefs = chefs.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()) ||
                                          c.Surname.ToLower().Contains(searchTerm.ToLower()) ||
                                          (c.Positions != null && c.Positions.Any(p => p.ToLower().Contains(searchTerm.ToLower()))) ||
                                          (c.SocialMedia != null &&
                                           (c.SocialMedia.FacebookUrl.ToLower().Contains(searchTerm.ToLower()) ||
                                            c.SocialMedia.TwitterUrl.ToLower().Contains(searchTerm.ToLower()) ||
                                            c.SocialMedia.InstagramUrl.ToLower().Contains(searchTerm.ToLower()))));
            }
            chefs = chefs.OrderByDescending(e => e.Id);

            return View(chefs);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            ChefVM chefVM = await GetApiResponse<ChefVM>($"api/Chef/getbyid/{id}");
            return View(chefVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseURL}/api/chef/delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "An error occurred while deleting the item." });
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var positions = await GetApiResponse<IEnumerable<PositionVM>>("api/Position/GetAll");
            var availablePositions = positions.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Title
            }).ToList();

            var model = new ChefCreateVM
            {
                AvailablePositions = availablePositions,
                SelectedPositions = new List<int>()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChefCreateVM model)
        {
            var positions = await GetApiResponse<IEnumerable<PositionVM>>("api/Position/GetAll");
            var availablePositions = positions.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Title
            }).ToList();
            model.AvailablePositions = availablePositions;

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Form məlumatları düzgün daxil edilməyib.";
                return View(model);
            }

            var allowedImageTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };

            using (var httpClient = new HttpClient())
            {
                using (var multipartContent = new MultipartFormDataContent())
                {
                    multipartContent.Add(new StringContent(model.Name), "Name");
                    multipartContent.Add(new StringContent(model.Surname), "Surname");

                    if (model.SelectedPositions != null && model.SelectedPositions.Any())
                    {
                        foreach (var positionId in model.SelectedPositions)
                        {
                            multipartContent.Add(new StringContent(positionId.ToString()), "PositionIds");
                        }
                    }

                    if (model.SocialMedia != null)
                    {
                        if (!string.IsNullOrEmpty(model.SocialMedia.FacebookUrl))
                            multipartContent.Add(new StringContent(model.SocialMedia.FacebookUrl), "SocialMedia.FacebookUrl");

                        if (!string.IsNullOrEmpty(model.SocialMedia.TwitterUrl))
                            multipartContent.Add(new StringContent(model.SocialMedia.TwitterUrl), "SocialMedia.TwitterUrl");

                        if (!string.IsNullOrEmpty(model.SocialMedia.InstagramUrl))
                            multipartContent.Add(new StringContent(model.SocialMedia.InstagramUrl), "SocialMedia.InstagramUrl");
                    }

                    if (model.Photos != null)
                    {
                        if (!allowedImageTypes.Contains(model.Photos.ContentType))
                        {
                            ModelState.AddModelError(string.Empty, "Dəstəklənməyən şəkil formatı. Yalnız JPEG, PNG, GIF və WEBP qəbul edilir.");
                            return View(model);
                        }

                        var fileContent = new StreamContent(model.Photos.OpenReadStream());
                        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.Photos.ContentType);
                        multipartContent.Add(fileContent, "Photos", model.Photos.FileName);
                    }

                    using (var response = await httpClient.PostAsync($"{BaseURL}/api/chef/create", multipartContent))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                        {
                            if (apiResponse.Contains("şəkil ilə mövcuddur"))
                            {
                                ModelState.AddModelError(string.Empty, "Bu ad və soyad ilə eyni şəkil mövcuddur.");
                                return View(model);
                            }

                            Console.WriteLine($"API Error: {apiResponse}");
                            ModelState.AddModelError(string.Empty, "API-də xəta baş verdi.");
                            return View(model);
                        }
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ChefVM chefVM = await GetApiResponse<ChefVM>($"api/Chef/getbyid/{id}");
            if (chefVM == null)
            {
                return NotFound();
            }
            var positions = await GetApiResponse<IEnumerable<PositionVM>>("api/Position/GetAll");
            var availablePositions = positions.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Title
            }).ToList();
            var model = new ChefEditVM
            {
                Id = chefVM.Id,
                Name = chefVM.Name,
                Surname = chefVM.Surname,
                SocialMedia = chefVM.SocialMedia,
                AvailablePositions = availablePositions,
                SelectedPositions = new List<int>()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ChefEditVM model)
        {
            ModelState.Clear();
            var positions = await GetApiResponse<IEnumerable<PositionVM>>("api/Position/GetAll");
            var availablePositions = positions.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Title
            }).ToList();
            model.AvailablePositions = availablePositions;

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Form məlumatları düzgün daxil edilməyib.";
                return View(model);
            }

            var existingChef = await GetApiResponse<ChefVM>($"api/Chef/getbyid/{model.Id}");
            var name = string.IsNullOrEmpty(model.Name) ? existingChef.Name : model.Name.Trim();
            var surname = string.IsNullOrEmpty(model.Surname) ? existingChef.Surname : model.Surname.Trim();
            var socialMedia = model.SocialMedia ?? existingChef.SocialMedia;

            using (var multipartContent = new MultipartFormDataContent())
            {
                multipartContent.Add(new StringContent(name), "Name");
                multipartContent.Add(new StringContent(surname), "Surname");
                if (socialMedia != null)
                {
                    if (!string.IsNullOrEmpty(socialMedia.FacebookUrl))
                        multipartContent.Add(new StringContent(socialMedia.FacebookUrl), "SocialMedia.FacebookUrl");

                    if (!string.IsNullOrEmpty(socialMedia.TwitterUrl))
                        multipartContent.Add(new StringContent(socialMedia.TwitterUrl), "SocialMedia.TwitterUrl");

                    if (!string.IsNullOrEmpty(socialMedia.InstagramUrl))
                        multipartContent.Add(new StringContent(socialMedia.InstagramUrl), "SocialMedia.InstagramUrl");
                }


                if (model.SelectedPositions != null && model.SelectedPositions.Any())
                {
                    foreach (var positionId in model.SelectedPositions)
                    {
                        multipartContent.Add(new StringContent(positionId.ToString()), "PositionIds");
                    }
                }


                if (model.Photos != null && model.Photos.Any())
                {
                    foreach (var photo in model.Photos)
                    {
                        var fileContent = new StreamContent(photo.OpenReadStream());
                        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(photo.ContentType);
                        multipartContent.Add(fileContent, "Photos", photo.FileName);
                    }
                }

                var response = await _httpClient.PutAsync($"{BaseURL}/api/chef/edit/{model.Id}", multipartContent);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Chef yenilənərkən xəta baş verdi.";
                    return View(model);
                }
            }

            return RedirectToAction(nameof(Index));
        }





    }
}