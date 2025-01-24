using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Models;
using SteakInMCV.ViewModels;

namespace SteakInMCV.Controllers
{
    public class BookATableController : Controller
    {
        private readonly string _baseUrl = "http://localhost:7031";

        private async Task<T> FetchDataFromApi<T>(string endpoint)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/api/{endpoint}");
            if (!response.IsSuccessStatusCode) return default;

            string apiResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(apiResponse);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var bookATableVM = TempData["BookATableVM"] != null
                ? JsonConvert.DeserializeObject<BookATableVM>((string)TempData["BookATableVM"])
                : new BookATableVM();

            bookATableVM.Testimonials = await FetchDataFromApi<IEnumerable<Testimonial>>("Testimonial/GetAll") ?? Enumerable.Empty<Testimonial>();
            var settings = await FetchDataFromApi<IEnumerable<Setting>>("Setting/GetAll");
            bookATableVM.Settings = settings?.ToDictionary(s => s.Key, s => s.Value) ?? new Dictionary<string, string>();
            bookATableVM.Banners = await FetchDataFromApi<List<Banner>>("Banner/GetAll") ?? new List<Banner>();
            bookATableVM.RestaurantTables = await FetchDataFromApi<IEnumerable<RestaurantTable>>("RestaurantTable/GetAll") ?? Enumerable.Empty<RestaurantTable>();

            ViewData["DateList"] = GenerateDateList();
            ViewData["TimeList"] = GenerateTimeList();

            return View(bookATableVM);
        }

        [HttpPost]
        public IActionResult SaveTempData(BookATableVM model)
        {
            TempData["BookATableVM"] = JsonConvert.SerializeObject(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> MakeReservation(BookATableVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["Error"] = string.Join(", ", errors);
                return RedirectToAction("Index");
            }

            var reservation = new Reservation
            {
                PeopleCount = model.Capacity,
                Date = model.Date,
                Time = TimeSpan.Parse(model.Time),
                ContactForm = model.ContactFormModel
            };


            using var client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_baseUrl}/api/Reservations/CreateReservation/create", content);

            TempData[response.IsSuccessStatusCode ? "Success" : "Error"] = response.IsSuccessStatusCode
                ? "Rezervasiya uğurla tamamlandı!"
                : "Rezervasiya uğursuz oldu.";
            return RedirectToAction("Index");
        }

        private static List<DateTime> GenerateDateList() =>
            Enumerable.Range(0, 7).Select(i => DateTime.Today.AddDays(i)).ToList();

        private static List<string> GenerateTimeList() =>
            Enumerable.Range(10, 14).Select(hour => new TimeSpan(hour, 0, 0).ToString(@"hh\:mm")).ToList();
    }
}
