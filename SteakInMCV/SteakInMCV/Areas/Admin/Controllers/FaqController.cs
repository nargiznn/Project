using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.AwardLogo;
using SteakInMCV.Areas.Admin.ViewModels.Faq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FaqController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Index()
        {
            IEnumerable<FaqVM> faqVMs = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/faq/getall"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    faqVMs = JsonConvert.DeserializeObject<IEnumerable<FaqVM>>(apiResponse);
                }
            }
            return View(faqVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            FaqVM faq = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/Faq/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    faq = JsonConvert.DeserializeObject<FaqVM>(apiResponse);
                }
            }

            return View(faq);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            FaqVM faq = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/Faq/getbyid/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    faq = JsonConvert.DeserializeObject<FaqVM>(apiResponse);
                }
            }

            return View(new FaqEditVM { Id = faq.Id, Question = faq.Question, Answer = faq.Answer, IsActive=faq.IsActive });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FaqEditVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            FaqVM existingFaq = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/admin/faq/getbyid/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    existingFaq = JsonConvert.DeserializeObject<FaqVM>(apiResponse);
                }
            }

            if (existingFaq == null)
            {
                return NotFound();
            }

            existingFaq.Question = request.Question ?? existingFaq.Question;
            existingFaq.Answer = request.Answer ?? existingFaq.Answer;
            existingFaq.IsActive = request.IsActive.HasValue ? request.IsActive.Value : false; 

            existingFaq.UpdatedAt = DateTime.UtcNow;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(existingFaq), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{BaseURl}/api/admin/faq/edit/{id}", content))
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
                using (var response = await httpClient.DeleteAsync($"{BaseURl}/api/admin/faq/delete/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new FaqCreateVM
            {
                IsActive = false 
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FaqCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync($"{BaseURl}/api/admin/faq/create", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError(string.Empty, "API'de bir xeta oldu.");
                        return View(request);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }







    }
}

