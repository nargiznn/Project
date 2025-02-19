using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.Award;
using SteakInMCV.Areas.Admin.ViewModels.Event;
using SteakInMCV.Areas.Admin.ViewModels.Setting;
using SteakInMCV.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        private readonly HttpClient _httpClient;

        public SettingController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        private async Task<T> GetApiResponse<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{BaseURl}/{endpoint}");
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(apiResponse);
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<SettingVM> settingVMs = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/setting/getall"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    settingVMs = JsonConvert.DeserializeObject<IEnumerable<SettingVM>>(apiResponse);
                }
            }
            return View(settingVMs);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            SettingVM award = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/Setting/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    award = JsonConvert.DeserializeObject<SettingVM>(apiResponse);
                }
            }

            return View(award);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var setting = await GetApiResponse<SettingVM>($"api/Setting/getbyid/{id}");
            var model = new SettingEditVM
            {
                Id = setting.Id,
                Value = setting.Value,
                ExistingImage = setting.Image 
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SettingEditVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Form məlumatları düzgün daxil edilməyib.";
                return View(model);
            }

            using (var multipartContent = new MultipartFormDataContent())
            {
                multipartContent.Add(new StringContent(model.Value ?? string.Empty), "Value");
                if (model.Image != null)
                {
                    var fileContent = new StreamContent(model.Image.OpenReadStream());
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.Image.ContentType);
                    multipartContent.Add(fileContent, "ImageFile", model.Image.FileName);
                }

                var response = await _httpClient.PutAsync($"{BaseURl}/api/Setting/edit/{model.Id}", multipartContent);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Setting məlumatları yenilənərkən xəta baş verdi.";
                    return View(model);
                }

                TempData["Success"] = "Setting uğurla yeniləndi.";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}

