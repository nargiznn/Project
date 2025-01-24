using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.Award;
using SteakInMCV.Areas.Admin.ViewModels.Slider;

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AwardController: Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Index()
        {
            IEnumerable<AwardVM> awardVMs = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/award/getall"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    awardVMs = JsonConvert.DeserializeObject<IEnumerable<AwardVM>>(apiResponse);
                }
            }
            return View(awardVMs);
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
                return View();
            }

            if (request.Photo != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(request.Photo.FileName);
                string extension = Path.GetExtension(request.Photo.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/img", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Photo.CopyToAsync(fileStream);
                }

                request.PhotoPath = "/admin/img/" + fileName;
            }

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync($"{BaseURl}/api/slider/create", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
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
        public async Task<IActionResult> Edit(int id)
        {
            AwardVM award = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/award/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    award = JsonConvert.DeserializeObject<AwardVM>(apiResponse);
                }
            }

            return View(new AwardVM { Id = award.Id, Name = award.Name, Year = award.Year });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AwardVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{BaseURl}/api/slider/edit/{id}", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            AwardVM award = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/award/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    award = JsonConvert.DeserializeObject<AwardVM>(apiResponse);
                }
            }

            return View(award);
        }
    }
}

