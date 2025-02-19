using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.Award;
using SteakInMCV.Areas.Admin.ViewModels.AwardLogo;

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AwardController : Controller
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string BaseURl = "http://localhost:7031";

        public async Task<IActionResult> Index()
        {
            IEnumerable<AwardVM> awardVMs = null;
            using (var response = await _httpClient.GetAsync($"{BaseURl}/api/admin/award/getall"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                awardVMs = JsonConvert.DeserializeObject<IEnumerable<AwardVM>>(apiResponse);
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
        public async Task<IActionResult> Create(AwardCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            request.Name = request.Name?.Trim();
            request.Year = request.Year?.Trim();
            DateTime awardYear;
            if (string.IsNullOrEmpty(request.Year) || !DateTime.TryParseExact(request.Year, "yyyy", null, System.Globalization.DateTimeStyles.None, out awardYear))
            {
                ModelState.AddModelError(string.Empty, "Daxil edilən il düzgün formatda deyil.");
                return View(request); 
            }

            if (awardYear > DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Daxil edilən il gələcək il olamaz!");
                return View(request); 
            }

            AwardVM duplicateAward = null;
            using (var response = await _httpClient.GetAsync($"{BaseURl}/api/admin/award/checkduplicate?name={request.Name}&year={request.Year}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ModelState.AddModelError(string.Empty, apiResponse);
                    return View(request);
                }

                duplicateAward = JsonConvert.DeserializeObject<AwardVM>(apiResponse);
            }

            if (duplicateAward != null)
            {
                ModelState.AddModelError(string.Empty, "Bu ad və il ilə artıq mövcud bir mükafat var.");
                return View(request); 
            }
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            using (var response = await _httpClient.PostAsync($"{BaseURl}/api/admin/award/create", content))
            {
                if (!response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Xəta baş verdi: {apiResponse}");
                    return View(request);
                }
            }
            return RedirectToAction(nameof(Index));
        }




        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var response = await _httpClient.DeleteAsync($"{BaseURl}/api/admin/award/delete/" + id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            AwardVM award = null;
            using (var response = await _httpClient.GetAsync($"{BaseURl}/api/admin/award/getbyid/" + id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                award = JsonConvert.DeserializeObject<AwardVM>(apiResponse);
            }

            string formattedYear = null;
            if (DateTime.TryParseExact(award.Year, "yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime year))
            {
                formattedYear = year.ToString("yyyy");
            }

            return View(new AwardEditVM
            {
                Id = award.Id,
                Name = award.Name?.Trim(),
                Year = formattedYear
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AwardEditVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            AwardVM existingAward = null;
            using (var response = await _httpClient.GetAsync($"{BaseURl}/api/admin/award/getbyid/" + id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                existingAward = JsonConvert.DeserializeObject<AwardVM>(apiResponse);
            }

            if (existingAward == null)
            {
                return NotFound();
            }

            request.Name = string.IsNullOrWhiteSpace(request.Name) ? existingAward.Name : request.Name?.Trim();

            string newAwardYearString = request.Year;
            if (string.IsNullOrEmpty(newAwardYearString))
            {
                newAwardYearString = existingAward.Year;
            }

            DateTime newAwardYear;
            if (DateTime.TryParseExact(newAwardYearString, "yyyy", null, System.Globalization.DateTimeStyles.None, out newAwardYear))
            {

            }
            else
            {
                newAwardYear = DateTime.Now;
            }

           
            if (newAwardYear > DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Daxil edilən il gələcək il olamaz!");
                return View(request); 
            }
            AwardVM duplicateAward = null;
            using (var response = await _httpClient.GetAsync($"{BaseURl}/api/admin/award/checkduplicate?name={request.Name}&year={newAwardYear.ToString("yyyy")}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                duplicateAward = JsonConvert.DeserializeObject<AwardVM>(apiResponse);
            }

            if (duplicateAward != null && duplicateAward.Id != id) 
            {
                ModelState.AddModelError(string.Empty, "Bu ad və il ilə artıq mövcud bir mükafat var.");
                return View(request);  
            }

            var updatedAward = new AwardVM
            {
                Id = existingAward.Id,
                Name = request.Name,
                Year = newAwardYear.ToString("yyyy")
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(updatedAward), Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PutAsync($"{BaseURl}/api/admin/award/edit/{id}", content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            AwardVM award = null;
            using (var response = await _httpClient.GetAsync($"{BaseURl}/api/admin/award/getbyid/" + id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                award = JsonConvert.DeserializeObject<AwardVM>(apiResponse);
            }

            return View(award);
        }
    }
}
