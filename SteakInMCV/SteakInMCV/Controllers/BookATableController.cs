using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Models;
using SteakInMCV.ViewModels;
using SteakInMCV.ViewModels.Events;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Controllers
{
    public class BookATableController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Index()
        {
            BookATableVM bookATableVM = new BookATableVM();
            using (var client = new HttpClient())
            {
                try
                {

                    var testimonialResponse = await client.GetAsync($"{BaseURl}/api/Testimonial/GetAll");
                    if (testimonialResponse.IsSuccessStatusCode)
                    {
                        string customerApiResponse = await testimonialResponse.Content.ReadAsStringAsync();
                        bookATableVM.Testimonials = (IEnumerable<Testimonial>)JsonConvert.DeserializeObject<IEnumerable<Testimonial>>(customerApiResponse);

                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + testimonialResponse.StatusCode;
                        bookATableVM.Testimonials = new List<Testimonial>();
                    }




                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        bookATableVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }

                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        bookATableVM.Settings = new Dictionary<string, string>();
                    }
                    var bannerResponse = await client.GetAsync($"{BaseURl}/api/banner/GetAll");
                    if (bannerResponse.IsSuccessStatusCode)
                    {
                        string bannerApiResponse = await bannerResponse.Content.ReadAsStringAsync();
                        bookATableVM.Banners = JsonConvert.DeserializeObject<List<Banner>>(bannerApiResponse).ToList();
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + bannerResponse.StatusCode;
                        bookATableVM.Banners = new List<Banner>();
                    }

                    var tableResponse = await client.GetAsync($"{BaseURl}/api/RestaurantTable/GetAll");
                    if (tableResponse.IsSuccessStatusCode)
                    {
                        string tableApiResponse = await tableResponse.Content.ReadAsStringAsync();
                        bookATableVM.RestaurantTables = JsonConvert.DeserializeObject<IEnumerable<RestaurantTable>>(tableApiResponse);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + tableResponse.StatusCode;
                        bookATableVM.RestaurantTables = new List<RestaurantTable>();
                    }

                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failes:{ex.Message}";
                }


            }
            var today = DateTime.Today;
            var dateList = Enumerable.Range(0, 7)
                                     .Select(i => today.AddDays(i))
                                     .ToList();
            ViewData["DateList"] = dateList;

            var startTime = new TimeSpan(10, 0, 0);
            var endTime = new TimeSpan(23, 0, 0); 
            var timeList = Enumerable.Range(0, (int)(endTime - startTime).TotalHours + 1)
                                     .Select(i => startTime.Add(TimeSpan.FromHours(i)).ToString(@"hh\:mm"))
                                     .ToList();
            ViewData["TimeList"] = timeList;

            return View(bookATableVM);
        }

    }
}

