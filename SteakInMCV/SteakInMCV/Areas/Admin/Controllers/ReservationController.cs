using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.Award;
using SteakInMCV.Areas.Admin.ViewModels.Reservation;
using SteakInMCV.Models;
using SteakInMCV.Models.Enum;
using SteakInMCV.ViewModels;

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReservationController:Controller
	{
        private readonly string BaseURl = "http://localhost:7031"; 
        public async Task<IActionResult> Index()
        {
            IEnumerable<ReservationViewModel> reservationVMs = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/reservations/GetReservations"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    reservationVMs = JsonConvert.DeserializeObject<IEnumerable<ReservationViewModel>>(apiResponse);
                }
            }
            return View(reservationVMs);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            ReservationViewModel reservation = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/Reservations/GetReservationById/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    reservation = JsonConvert.DeserializeObject<ReservationViewModel>(apiResponse);
                }
            }
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, ReservationStatus status)
        {
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(status), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync($"{BaseURl}/api/Reservations/UpdateReservationStatus/update-status/{id}", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        return BadRequest(apiResponse);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            ReservationViewModel award = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{BaseURl}/api/Reservations/GetReservationById/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    award = JsonConvert.DeserializeObject<ReservationViewModel>(apiResponse);
                }
            }

            return View(award);
        }
    }
}

