using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.Tag;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Index()
        {
            IEnumerable<TagVM> categoryVMs = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/Tag/getall"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVMs = JsonConvert.DeserializeObject<IEnumerable<TagVM>>(apiResponse);
                }
            }
            return View(categoryVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            TagVM categoryVM = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/Tag/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVM = JsonConvert.DeserializeObject<TagVM>(apiResponse);
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
        public async Task<IActionResult> Create(TagCreateVM request)
        {
            request.Name = request.Name?.Trim();

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                ModelState.AddModelError(string.Empty, "Ad boş buraxıla bilməz.");
                return View(request);
            }
            IEnumerable<TagVM> existingCategories = await GetAllCategoriesAsync();

            if (existingCategories.Any(c => c.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(string.Empty, "Eyni adda artıq xüsusi kateqoriya mövcuddur.");
                return View(request);
            }

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync($"{BaseURl}/api/admin/Tag/create", content))
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

        private async Task<IEnumerable<TagVM>> GetAllCategoriesAsync()
        {
            IEnumerable<TagVM> categoryVMs = new List<TagVM>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{BaseURl}/api/admin/Tag/getall");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVMs = JsonConvert.DeserializeObject<IEnumerable<TagVM>>(apiResponse);
                }
            }
            return categoryVMs;
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            TagVM categoryVM = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/Tag/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVM = JsonConvert.DeserializeObject<TagVM>(apiResponse);
                }
            }

            if (categoryVM == null)
            {
                return NotFound();
            }

            return View(new TagEditVM { Id = categoryVM.Id, Name = categoryVM.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TagEditVM request)
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
            IEnumerable<TagVM> existingCategories = await GetAllCategoriesAsync();

            if (existingCategories.Any(c => c.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase) && c.Id != id))
            {
                ModelState.AddModelError(string.Empty, "Eyni adda artıq tag mövcuddur.");
                return View(request);
            }

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{BaseURl}/api/admin/Tag/edit/{id}", content))
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

        private async Task<TagVM> GetCategoryByIdAsync(int id)
        {
            TagVM categoryVM = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/Tag/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categoryVM = JsonConvert.DeserializeObject<TagVM>(apiResponse);
                }
            }
            return categoryVM;
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{BaseURl}/api/admin/Tag/delete/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

    }
}

