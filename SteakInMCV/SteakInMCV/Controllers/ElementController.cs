using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Models;
using SteakInMCV.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Controllers
{
    public class ElementController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Client()
        {
            ElementVM elementVM = new ElementVM();
            using (var client = new HttpClient())
            {
                try
                {
                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        elementVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        elementVM.Settings = new Dictionary<string, string>();
                    }


                    var clientResponse = await client.GetAsync($"{BaseURl}/api/client/GetAll");
                    if (clientResponse.IsSuccessStatusCode)
                    {
                        string clinetApiResponse = await clientResponse.Content.ReadAsStringAsync();
                        elementVM.Clients = (IEnumerable<Client>)JsonConvert.DeserializeObject<IEnumerable<Client>>(clinetApiResponse);

                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + clientResponse.StatusCode;
                        elementVM.Clients = new List<Client>();
                    }

                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failed: {ex.Message}";
                }

            }

            return View("Client", elementVM);
        }

        public async Task<IActionResult> Counter()
        {
            ElementVM elementVM = new ElementVM();
            using (var client = new HttpClient())
            {
                try
                {
                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        elementVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        elementVM.Settings = new Dictionary<string, string>();
                    }

                    var statisticResponse = await client.GetAsync($"{BaseURl}/api/statistic/GetAll");
                    if (statisticResponse.IsSuccessStatusCode)
                    {
                        string statisticApiResponse = await statisticResponse.Content.ReadAsStringAsync();
                        var statistic = JsonConvert.DeserializeObject<IEnumerable<Statistic>>(statisticApiResponse);
                        elementVM.Statistics = statistic.ToDictionary(s => s.Title, s => s.Value);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + statisticResponse.StatusCode;
                        elementVM.Statistics = new Dictionary<string, int>();
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failed: {ex.Message}";
                }

            }

            return View("Counter", elementVM);
        }


        public async Task<IActionResult> Pricing()
        {
            ElementVM elementVM = new ElementVM();
            using (var client = new HttpClient())
            {
                try
                {
                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        elementVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        elementVM.Settings = new Dictionary<string, string>();
                    }

                    var mealPackageResponse = await client.GetAsync($"{BaseURl}/api/mealPackage/GetAll");
                    if (mealPackageResponse.IsSuccessStatusCode)
                    {
                        string mealPackageApiResponse = await mealPackageResponse.Content.ReadAsStringAsync();
                        elementVM.MealPackages =(IEnumerable<MealPackage>)JsonConvert.DeserializeObject<IEnumerable<MealPackage>>(mealPackageApiResponse);

                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + mealPackageResponse.StatusCode;
                        elementVM.MealPackages = new List<MealPackage>();
                    }

                    var lunchSetResponse = await client.GetAsync($"{BaseURl}/api/lunchSet/GetAll");
                    if (lunchSetResponse.IsSuccessStatusCode)
                    {
                        string lunchSetApiResponse = await lunchSetResponse.Content.ReadAsStringAsync();
                        elementVM.LunchSets = (IEnumerable<LunchSet>)JsonConvert.DeserializeObject<IEnumerable<LunchSet>>(lunchSetApiResponse);

                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + lunchSetResponse.StatusCode;
                        elementVM.LunchSets = new List<LunchSet>();
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failed: {ex.Message}";
                }

            }

            return View("Pricing", elementVM);
        }
    }
}

