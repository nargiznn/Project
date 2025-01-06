using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.Slider;
using SteakInMCV.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly string BaseURl = "http://localhost:5073";
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
            SliderVM slider = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/slider/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    slider = JsonConvert.DeserializeObject<SliderVM>(apiResponse);
                }
            }

            return View(new SliderEditVM { Id = slider.Id, Title = slider.Title, MainTitle = slider.MainTitle, BtnText = slider.BtnText,Desc=slider.Desc ,Photo = slider.Photo });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SliderEditVM request)
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

