using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.Slider;
using SteakInMCV.Models;

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";

        public async Task<IActionResult> Index()
        {
            IEnumerable<SliderVM> sliders = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/slider/getall"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    sliders = JsonConvert.DeserializeObject<IEnumerable<SliderVM>>(apiResponse);
                }
            }
            return View(sliders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            using (var httpClient = new HttpClient())
            {
                using (var multipartContent = new MultipartFormDataContent())
                {
                    multipartContent.Add(new StringContent(request.Title), "Title");
                    multipartContent.Add(new StringContent(request.MainTitle), "MainTitle");
                    multipartContent.Add(new StringContent(request.Desc), "Desc");
                    multipartContent.Add(new StringContent(request.BtnText), "BtnText");

                    if (request.file != null)
                    {
                        if (!request.file.ContentType.StartsWith("image/"))
                        {
                            ModelState.AddModelError("file", "Yüklənən fayl şəkil formatında olmalıdır.");
                            return View(request);
                        }

                        if (request.file.Length > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("file", "Şəkil ölçüsü maksimum 5 MB olmalıdır.");
                            return View(request);
                        }

                        var fileContent = new StreamContent(request.file.OpenReadStream());
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(request.file.ContentType);
                        multipartContent.Add(fileContent, "file", request.file.FileName);
                    }

                    using (var response = await httpClient.PostAsync($"{BaseURl}/api/slider/create", multipartContent))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            TempData["Error"] = "Slider yaradılarkən xəta baş verdi.";
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
            SliderVM slider = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/slider/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    slider = JsonConvert.DeserializeObject<SliderVM>(apiResponse);
                }
            }

            return View(new SliderEditVM
            {
                Id = slider.Id,
                Title = slider.Title,
                MainTitle = slider.MainTitle,
                BtnText = slider.BtnText,
                Desc = slider.Desc
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SliderEditVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            SliderVM existingSlider;

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{BaseURl}/api/slider/getbyid/" + id);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Məlumat tapılmadı.";
                    return RedirectToAction(nameof(Index));
                }

                string apiResponse = await response.Content.ReadAsStringAsync();
                existingSlider = JsonConvert.DeserializeObject<SliderVM>(apiResponse);
            }
            using (var httpClient = new HttpClient())
            {
                using (var multipartContent = new MultipartFormDataContent())
                {
                    multipartContent.Add(new StringContent(request.Title ?? existingSlider.Title), "Title");
                    multipartContent.Add(new StringContent(request.MainTitle ?? existingSlider.MainTitle), "MainTitle");
                    multipartContent.Add(new StringContent(request.Desc ?? existingSlider.Desc), "Desc");
                    multipartContent.Add(new StringContent(request.BtnText ?? existingSlider.BtnText), "BtnText");

                    if (request.File != null)
                    {
                        if (!request.File.ContentType.StartsWith("image/"))
                        {
                            ModelState.AddModelError("File", "Yüklənən fayl şəkil formatında olmalıdır.");
                            return View(request);
                        }

                        if (request.File.Length > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("File", "Şəkil ölçüsü maksimum 5 MB olmalıdır.");
                            return View(request);
                        }

                        var fileContent = new StreamContent(request.File.OpenReadStream());
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(request.File.ContentType);
                        multipartContent.Add(fileContent, "file", request.File.FileName);
                    }
                    else
                    {
                        multipartContent.Add(new StringContent(existingSlider.Image ?? ""), "file");
                    }

                    var response = await httpClient.PutAsync($"{BaseURl}/api/slider/edit/{id}", multipartContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        TempData["Error"] = "Slider yenilənərkən xəta baş verdi.";
                        return View(request);
                    }
                }
            }


            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{BaseURl}/api/slider/delete/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

       

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            SliderVM slider = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/slider/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    slider = JsonConvert.DeserializeObject<SliderVM>(apiResponse);
                }
            }

            return View(slider);
        }
    }
}