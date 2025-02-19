using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.Faq;
using SteakInMCV.Areas.Admin.ViewModels.MenuCategory;
using SteakInMCV.Areas.Admin.ViewModels.Slider;
using SteakInMCV.Areas.Admin.ViewModels.SpecialCategory;
using SteakInMCV.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuCategoryController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Index()
        {
            IEnumerable<MenuCategoryVM> sliders = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/MenuCategory/getall"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    sliders = JsonConvert.DeserializeObject<IEnumerable<MenuCategoryVM>>(apiResponse);
                }
            }
            return View(sliders);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            MenuCategoryVM faq = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/MenuCategory/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    faq = JsonConvert.DeserializeObject<MenuCategoryVM>(apiResponse);
                }
            }

            return View(faq);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new MenuCategoryCreateVM
            {
                IsActive = false
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuCategoryCreateVM request)
        {
            request.Name = request.Name?.Trim();

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                ModelState.AddModelError(string.Empty, "Ad boş buraxıla bilməz.");
                return View(request);
            }

            IEnumerable<MenuCategoryVM> existingCategories = await GetAllCategoriesAsync();

            if (existingCategories.Any(c => c.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(string.Empty, "Eyni adda artıq menyu kateqoriya mövcuddur.");
                return View(request);
            }

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync($"{BaseURl}/api/admin/MenuCategory/create", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError(string.Empty, "API-də bir xəta baş verdi.");
                        return View(request);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<MenuCategoryVM>> GetAllCategoriesAsync()
        {
            IEnumerable<MenuCategoryVM> menuCategoryVMs = new List<MenuCategoryVM>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{BaseURl}/api/admin/MenuCategory/getall");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    menuCategoryVMs = JsonConvert.DeserializeObject<IEnumerable<MenuCategoryVM>>(apiResponse);
                }
            }
            return menuCategoryVMs;
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            MenuCategoryVM menuCategoryVM = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/MenuCategory/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    menuCategoryVM = JsonConvert.DeserializeObject<MenuCategoryVM>(apiResponse);
                }
            }

            if (menuCategoryVM == null)
            {
                return NotFound();
            }

            return View(new MenuCategoryEditVM { Id = menuCategoryVM.Id, Name = menuCategoryVM.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuCategoryEditVM request)
        {
            request.Name = request.Name?.Trim();
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                var existingCategory = await GetCategoryByIdAsync(id);
                request.Name = existingCategory?.Name;
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                ModelState.AddModelError(string.Empty, "Ad boş buraxıla bilməz.");
                return View(request);
            }

            IEnumerable<MenuCategoryVM> existingCategories = await GetAllCategoriesAsync();

            if (existingCategories.Any(c => c.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase) && c.Id != id))
            {
                ModelState.AddModelError(string.Empty, "Eyni adda artıq menyu kateqoriya mövcuddur.");
                return View(request);
            }

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{BaseURl}/api/admin/MenuCategory/edit/{id}", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError(string.Empty, "API-də bir xəta baş verdi.");
                        return View(request);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<MenuCategoryVM> GetCategoryByIdAsync(int id)
        {
            MenuCategoryVM menuCategoryVM = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/MenuCategory/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    menuCategoryVM = JsonConvert.DeserializeObject<MenuCategoryVM>(apiResponse);
                }
            }
            return menuCategoryVM;
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{BaseURl}/api/admin/MenuCategory/delete/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

