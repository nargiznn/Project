using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.Cuisine;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CuisineController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Index()
        {
            IEnumerable<CuisineVM> categoryVMs = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/Cuisine/getall"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVMs = JsonConvert.DeserializeObject<IEnumerable<CuisineVM>>(apiResponse);
                }
            }
            return View(categoryVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            CuisineVM categoryVM = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/Cuisine/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVM = JsonConvert.DeserializeObject<CuisineVM>(apiResponse);
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
        public async Task<IActionResult> Create(CuisineCreateVM request)
        {
            request.Name = request.Name?.Trim();
            request.Desc = request.Desc?.Trim();

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                ModelState.AddModelError(string.Empty, "Name boş buraxıla bilməz.");
                return View(request);
            }
            if (string.IsNullOrWhiteSpace(request.Desc))
            {
                ModelState.AddModelError(string.Empty, "Desc boş buraxıla bilməz.");
                return View(request);
            }
            IEnumerable<CuisineVM> existingCategories = await GetAllCategoriesAsync();

            if (existingCategories.Any(c => c.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(string.Empty, "Eyni name artıq mövcuddur.");
                return View(request);
            }
            if (existingCategories.Any(c => c.Desc.Equals(request.Desc, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(string.Empty, "Eyni desc artıq mövcuddur.");
                return View(request);
            }
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync($"{BaseURl}/api/admin/Cuisine/create", content))
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

        private async Task<IEnumerable<CuisineVM>> GetAllCategoriesAsync()
        {
            IEnumerable<CuisineVM> categoryVMs = new List<CuisineVM>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{BaseURl}/api/admin/Cuisine/getall");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVMs = JsonConvert.DeserializeObject<IEnumerable<CuisineVM>>(apiResponse);
                }
            }
            return categoryVMs;
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CuisineVM categoryVM = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/Cuisine/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVM = JsonConvert.DeserializeObject<CuisineVM>(apiResponse);
                }
            }

            if (categoryVM == null)
            {
                return NotFound();
            }

            return View(new CuisineEditVM { Id = categoryVM.Id, Name = categoryVM.Name , Desc = categoryVM.Desc });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CuisineEditVM request)
        {
            request.Name = request.Name?.Trim();
            request.Desc = request.Desc?.Trim();
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                var existingCategory = await GetCategoryByIdAsync(id);
                request.Name = existingCategory?.Name;
            }
            if (string.IsNullOrWhiteSpace(request.Desc))
            {
                var existingCategory = await GetCategoryByIdAsync(id);
                request.Desc = existingCategory?.Desc;
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                ModelState.AddModelError(string.Empty, "Ad boş buraxıla bilməz.");
                return View(request);
            }
            if (string.IsNullOrWhiteSpace(request.Desc))
            {
                ModelState.AddModelError(string.Empty, "desc boş buraxıla bilməz.");
                return View(request);
            }
            IEnumerable<CuisineVM> existingCategories = await GetAllCategoriesAsync();

            if (existingCategories.Any(c => c.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase) && c.Id != id))
            {
                ModelState.AddModelError(string.Empty, "Eyni adda artıq xüsusi kateqoriya mövcuddur.");
                return View(request);
            }

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{BaseURl}/api/admin/Cuisine/edit/{id}", content))
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

        private async Task<CuisineVM> GetCategoryByIdAsync(int id)
        {
            CuisineVM categoryVM = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/Cuisine/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVM = JsonConvert.DeserializeObject<CuisineVM>(apiResponse);
                }
            }
            return categoryVM;
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{BaseURl}/api/admin/Cuisine/delete/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

    }
}

