using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.Position;
using SteakInMCV.Areas.Admin.ViewModels.Slider;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Index()
        {
            IEnumerable<PositionVM> sliders = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/Position/getall"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    sliders = JsonConvert.DeserializeObject<IEnumerable<PositionVM>>(apiResponse);
                }
            }
            return View(sliders);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            PositionVM faq = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/Position/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    faq = JsonConvert.DeserializeObject<PositionVM>(apiResponse);
                }
            }

            return View(faq);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new PositionCreateVM
            {
                IsActive = false
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PositionCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            bool isActive = request.IsActive ?? false; 

            using (var httpClient = new HttpClient())
            {
                using (var multipartContent = new MultipartFormDataContent())
                {
                    multipartContent.Add(new StringContent(request.Title), "Title");
                    multipartContent.Add(new StringContent(request.Description), "Description");
                    multipartContent.Add(new StringContent(isActive.ToString().ToLower()), "IsActive");

                    using (var response = await httpClient.PostAsync($"{BaseURl}/api/position/create", multipartContent))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            TempData["Error"] = "Position yaradılarkən xəta baş verdi.";
                            return View(request);
                        }
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }


        private async Task<IEnumerable<PositionVM>> GetAllCategoriesAsync()
        {
            IEnumerable<PositionVM> PositionVMs = new List<PositionVM>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{BaseURl}/api/Position/getall");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    PositionVMs = JsonConvert.DeserializeObject<IEnumerable<PositionVM>>(apiResponse);
                }
            }
            return PositionVMs;
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            PositionVM PositionVM = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/Position/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    PositionVM = JsonConvert.DeserializeObject<PositionVM>(apiResponse);
                }
            }

            if (PositionVM == null)
            {
                return NotFound();
            }

            return View(new PositionEditVM { Id = PositionVM.Id, Title = PositionVM.Title,Description=PositionVM.Description,IsActive=PositionVM.IsActive });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PositionEditVM request)
        {

            request.Title = request.Title?.Trim();
            request.Description = request.Description?.Trim();


            if (string.IsNullOrWhiteSpace(request.Title))
            {
                ModelState.AddModelError(string.Empty, "Ad boş buraxıla bilməz.");
                return View(request);
            }


            if (string.IsNullOrWhiteSpace(request.Description))
            {
                ModelState.AddModelError(string.Empty, "Açıqlama boş buraxıla bilməz.");
                return View(request);
            }


            bool isActive = request.IsActive ?? false; 


            IEnumerable<PositionVM> existingCategories = await GetAllCategoriesAsync();
            if (existingCategories.Any(c => c.Title.Equals(request.Title, StringComparison.OrdinalIgnoreCase) && c.Id != id))
            {
                ModelState.AddModelError(string.Empty, "Eyni adda mövcud olan başqa bir mövqe var.");
                return View(request);
            }


            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(request.Title), "Title");
            formData.Add(new StringContent(request.Description), "Description");
            formData.Add(new StringContent(isActive.ToString()), "IsActive");

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PutAsync($"{BaseURl}/api/Position/Edit/{id}", formData);
                string apiResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, $"API-də bir xəta baş verdi: {apiResponse}");
                    return View(request);
                }
            }

            return RedirectToAction(nameof(Index));
        }


        private async Task<PositionVM> GetCategoryByIdAsync(int id)
        {
            PositionVM PositionVM = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/Position/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    PositionVM = JsonConvert.DeserializeObject<PositionVM>(apiResponse);
                }
            }
            return PositionVM;
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{BaseURl}/api/Position/delete/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

