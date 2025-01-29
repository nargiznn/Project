using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Models;
using SteakInMCV.Models.Enum;
using SteakInMCV.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SteakInMCV.Controllers
{
    public class BookATableController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        private readonly HttpClient _httpClient;

        public BookATableController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        private async Task<T> GetApiData<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var apiResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(apiResponse);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = $"API request failed: {ex.Message}";
                return default;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var homeVM = await PopulateViewModelData();

            return View(homeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BookATableVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please enter the information correctly.";
                model = await PopulateViewModelData();
                return View(model);
            }

            var reservation = new Reservation
            {
                Name = model.FirstName,
                Surname = model.LastName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                PeopleCount = model.Capacity,
                Date = model.Date,
                Time = model.Time,
                Status = ReservationStatus.Pending
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseURl}/api/Reservations/CreateReservation/create", reservation);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Reservation successfully created!";
                }
                else
                {
                    TempData["Error"] = "Reservation failed.";
                }
            }
            catch (HttpRequestException ex)
            {
                TempData["Error"] = $"API request failed: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }



        private async Task<BookATableVM> PopulateViewModelData()
        {
            var homeVM = new BookATableVM();

            homeVM.Banners = await GetApiData<List<Banner>>($"{BaseURl}/api/banner/GetAll") ?? new List<Banner>();
            homeVM.Testimonials = await GetApiData<IEnumerable<Testimonial>>($"{BaseURl}/api/Testimonial/GetAll") ?? new List<Testimonial>();
            var settings = await GetApiData<IEnumerable<Setting>>($"{BaseURl}/api/setting/GetAll");
            homeVM.Settings = settings?.ToDictionary(s => s.Key, s => s.Value) ?? new Dictionary<string, string>();

            return homeVM;
        }
    }
}
