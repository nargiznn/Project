using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.SpecialCategory;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialCategoryController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Index()
        {
            IEnumerable<SpecialCategoryVM> categoryVMs = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/SpecialCategory/getall"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVMs = JsonConvert.DeserializeObject<IEnumerable<SpecialCategoryVM>>(apiResponse);
                }
            }
            return View(categoryVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            SpecialCategoryVM categoryVM = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/SpecialCategory/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVM = JsonConvert.DeserializeObject<SpecialCategoryVM>(apiResponse);
                }
            }

            return View(categoryVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialCategoryCreateVM request)
        {
            request.Name = request.Name?.Trim();

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                ModelState.AddModelError(string.Empty, "Ad boş buraxıla bilməz.");
                return View(request);
            }
            IEnumerable<SpecialCategoryVM> existingCategories = await GetAllCategoriesAsync();

            if (existingCategories.Any(c => c.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(string.Empty, "Eyni adda artıq xüsusi kateqoriya mövcuddur.");
                return View(request);
            }

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync($"{BaseURl}/api/admin/specialcategory/create", content))
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

        private async Task<IEnumerable<SpecialCategoryVM>> GetAllCategoriesAsync()
        {
            IEnumerable<SpecialCategoryVM> categoryVMs = new List<SpecialCategoryVM>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{BaseURl}/api/admin/SpecialCategory/getall");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVMs = JsonConvert.DeserializeObject<IEnumerable<SpecialCategoryVM>>(apiResponse);
                }
            }
            return categoryVMs;
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            SpecialCategoryVM categoryVM = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/SpecialCategory/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVM = JsonConvert.DeserializeObject<SpecialCategoryVM>(apiResponse);
                }
            }

            if (categoryVM == null)
            {
                return NotFound();
            }

            return View(new SpecialCategoryEditVM { Id = categoryVM.Id, Name = categoryVM.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SpecialCategoryEditVM request)
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
            IEnumerable<SpecialCategoryVM> existingCategories = await GetAllCategoriesAsync();

            if (existingCategories.Any(c => c.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase) && c.Id != id))
            {
                ModelState.AddModelError(string.Empty, "Eyni adda artıq xüsusi kateqoriya mövcuddur.");
                return View(request);
            }

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{BaseURl}/api/admin/SpecialCategory/edit/{id}", content))
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

        private async Task<SpecialCategoryVM> GetCategoryByIdAsync(int id)
        {
            SpecialCategoryVM categoryVM = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/SpecialCategory/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVM = JsonConvert.DeserializeObject<SpecialCategoryVM>(apiResponse);
                }
            }
            return categoryVM;
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{BaseURl}/api/admin/specialcategory/delete/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }


    }
}

