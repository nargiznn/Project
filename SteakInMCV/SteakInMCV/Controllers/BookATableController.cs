using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.AwardLogo;
using SteakInMCV.Areas.Admin.ViewModels.Product;
using SteakInMCV.Models;
using SteakInMCV.Models.Enum;
using SteakInMCV.ViewModels;
using SteakInMCV.ViewModels.Events;

namespace SteakInMCV.Controllers
{
    public class BookATableController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";

        private async Task<T> GetApiData<T>(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(apiResponse);
                }
                else
                {
                    ViewData["Error"] = $"API request failed with status code: {response.StatusCode}";
                    return default(T);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            BookATableVM homeVM = new BookATableVM();

            try
            {
                homeVM.Testimonials = await GetApiData<IEnumerable<Testimonial>>($"{BaseURl}/api/Testimonial/GetAll");
                homeVM.Settings = (await GetApiData<IEnumerable<Setting>>($"{BaseURl}/api/setting/GetAll"))
                    .ToDictionary(s => s.Key, s => s.Value);
                homeVM.Banners = await GetApiData<List<Banner>>($"{BaseURl}/api/banner/GetAll");

                if (TempData["Error"] != null)
                {
                    ViewData["Error"] = TempData["Error"];
                }
                if (TempData["Success"] != null)
                {
                    ViewData["Success"] = TempData["Success"];
                }

                if (TempData["Errors"] != null)
                {
                    var errors = JsonConvert.DeserializeObject<string[]>(TempData["Errors"].ToString());
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = $"API request failed: {ex.Message}";
            }

            return View(homeVM);
        }
        [HttpPost]
        public async Task<IActionResult> CreateReservation(BookATableVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Errors"] = JsonConvert.SerializeObject(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray());
                    return RedirectToAction("Index");
                }

                using (var client = new HttpClient())
                {
                    var reservation = new Reservation
                    {
                        Name = model.ReservationForm.FirstName,
                        Surname = model.ReservationForm.LastName,
                        Email = model.ReservationForm.Email,
                        PhoneNumber = model.ReservationForm.Phone,
                        PeopleCount = model.Capacity,
                        Date = model.Date,
                        Time = TimeSpan.Parse(model.Time),
                        Status = ReservationStatus.Pending
                    };

                    var response = await client.PostAsJsonAsync($"{BaseURl}/api/Reservations/CreateReservation/create", reservation);

                    if (response.IsSuccessStatusCode)
                        TempData["Success"] = "Rezervasiya uğurla yaradıldı!"; 
                    else
                        TempData["Error"] = "Rezervasiya uğursuz oldu.";  
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Xəta baş verdi: {ex.Message}"; 
                return RedirectToAction("Index");
            }
        }

    }
}
