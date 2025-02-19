using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.AwardLogo;
using SteakInMCV.Areas.Admin.ViewModels.Chef;
using SteakInMCV.Areas.Admin.ViewModels.Position;
using SteakInMCV.Areas.Admin.ViewModels.Testimonial;
using SteakInMCV.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestimonialController : Controller
    {
        private readonly string BaseURL = "http://localhost:7031";
        private readonly HttpClient _httpClient;

        public TestimonialController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        private async Task<T> GetApiResponse<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{BaseURL}/{endpoint}");
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(apiResponse);
        }
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Index()
        {
            IEnumerable<TestimonialVM> awardLogo = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/Testimonial/GetAll"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    awardLogo = JsonConvert.DeserializeObject<IEnumerable<TestimonialVM>>(apiResponse);
                }
            }
            return View(awardLogo);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            TestimonialVM chefVM = await GetApiResponse<TestimonialVM>($"api/testimonial/getbyid/{id}");
            return View(chefVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseURL}/api/testimonial/delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "An error occurred while deleting the item." });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new TestimonialCreateVM
            {
                ReviewTypeList = Enum.GetValues(typeof(ReviewType))
                    .Cast<ReviewType>()
                    .Select(rt => new SelectListItem
                    {
                        Value = ((int)rt).ToString(),
                        Text = rt.ToString()
                    }).ToList()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestimonialCreateVM model)
        {
            if (ModelState.IsValid)
            {
                model.ReviewTypeList = Enum.GetValues(typeof(ReviewType))
                    .Cast<ReviewType>()
                    .Select(rt => new SelectListItem
                    {
                        Value = ((int)rt).ToString(),
                        Text = rt.ToString()
                    }).ToList();

                return View(model);
            }

            var allowedImageTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };

            using (var httpClient = new HttpClient())
            {
                using (var multipartContent = new MultipartFormDataContent())
                {
                    if (model.ReviewType == null || model.ReviewType == 0)
                    {
                        ModelState.AddModelError(nameof(model.ReviewType), "Zəhmət olmasa Review Type seçin.");
                        model.ReviewTypeList = Enum.GetValues(typeof(ReviewType))
                            .Cast<ReviewType>()
                            .Select(rt => new SelectListItem
                            {
                                Value = ((int)rt).ToString(),
                                Text = rt.ToString()
                            }).ToList();
                        return View(model);
                    }
                    multipartContent.Add(new StringContent(model.Name ?? string.Empty), "Name");
                    multipartContent.Add(new StringContent(model.Surname ?? string.Empty), "Surname");
                    multipartContent.Add(new StringContent(model.Text ?? string.Empty), "Text");
                    multipartContent.Add(new StringContent(model.Raiting.ToString()), "Raiting");
                    multipartContent.Add(new StringContent(model.ReviewType.ToString()), "ReviewType");

                   

                    if (model.file != null)
                    {
                        var fileContent = new StreamContent(model.file.OpenReadStream());
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.file.ContentType);
                        multipartContent.Add(fileContent, "file", model.file.FileName);
                    }

                    var response = await httpClient.PostAsync($"{BaseURL}/api/testimonial/create", multipartContent);
                    var apiResponse = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        TempData["Error"] = "API-də xəta baş verdi: " + apiResponse;
                        return View(model);
                    }
                }
            }


            TempData["Success"] = "Testimonial uğurla yaradıldı!";
            return RedirectToAction(nameof(Index));
        }



    }
}

