using System;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.AwardLogo;
using SteakInMCV.ViewModels;

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AwardLogoController: Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Index()
        {
            IEnumerable<AwardLogoVM> awardLogo = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/AwardLogo/GetAll"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    awardLogo = JsonConvert.DeserializeObject<IEnumerable<AwardLogoVM>>(apiResponse);
                }
            }
            return View(awardLogo);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{BaseURl}/api/AwardLogo/delete/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            AwardLogoVM awardLogo = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/AwardLogo/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    awardLogo = JsonConvert.DeserializeObject<AwardLogoVM>(apiResponse);
                }
            }

            return View(awardLogo);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AwardLogoCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            using (var httpClient = new HttpClient())
            {
                using (var multipartContent = new MultipartFormDataContent())
                {
                    multipartContent.Add(new StringContent(request.ImgUrl), "ImgUrl");
                    multipartContent.Add(new StringContent(request.AltText), "AltText");


                    if (request.Image != null)
                    {
                        var fileContent = new StreamContent(request.Image.OpenReadStream());
                        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(request.Image.ContentType);
                        multipartContent.Add(fileContent, "Image", request.Image.FileName);
                    }

                    using (var response = await httpClient.PostAsync($"{BaseURl}/api/AwardLogo/create", multipartContent))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                        {
                            ModelState.AddModelError(string.Empty, "API-də xəta baş verdi.");
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
            AwardLogoVM awardLogo = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/AwardLogo/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    awardLogo = JsonConvert.DeserializeObject<AwardLogoVM>(apiResponse);
                }
            }

            return View(new AwardLogoEditVM
            {
                Id = awardLogo.Id,
                ImgUrl = awardLogo.ImgUrl,
                AltText = awardLogo.AltText,
                //file = awardLogo.Image
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AwardLogoEditVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            AwardLogoVM existingAwardLogo = null;

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{BaseURl}/api/AwardLogo/getbyid/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Məlumat tapılmadı.";
                    return RedirectToAction(nameof(Index));
                }

                string apiResponse = await response.Content.ReadAsStringAsync();
                existingAwardLogo = JsonConvert.DeserializeObject<AwardLogoVM>(apiResponse);
            }

            using (var httpClient = new HttpClient())
            {
                using (var multipartContent = new MultipartFormDataContent())
                {
                    multipartContent.Add(new StringContent(request.ImgUrl ?? existingAwardLogo.ImgUrl), "ImgUrl");
                    multipartContent.Add(new StringContent(request.AltText ?? existingAwardLogo.AltText), "AltText");
                    if (request.file != null)
                    {
                        var fileContent = new StreamContent(request.file.OpenReadStream());
                        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(request.file.ContentType);
                        multipartContent.Add(fileContent, "Image", request.file.FileName);
                    }

                    var response = await httpClient.PutAsync($"{BaseURl}/api/AwardLogo/edit/{id}", multipartContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        TempData["Error"] = "Məlumat yenilənərkən xəta baş verdi.";
                        return View(request);
                    }
                }
            }


            return RedirectToAction(nameof(Index));
        }

      


    }
}

