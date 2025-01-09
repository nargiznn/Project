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


                    var customerResponse = await client.GetAsync($"{BaseURl}/api/Customer/GetAll");
                    if (customerResponse.IsSuccessStatusCode)
                    {
                        string customerApiResponse = await customerResponse.Content.ReadAsStringAsync();
                        bookATableVM.Customers = (IEnumerable<Customer>)JsonConvert.DeserializeObject<IEnumerable<Customer>>(customerApiResponse);

                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + customerResponse.StatusCode;
                        bookATableVM.Customers = new List<Customer>();
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

                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failes:{ex.Message}";
                }


            }
            return View(bookATableVM);
        }

    }
}

