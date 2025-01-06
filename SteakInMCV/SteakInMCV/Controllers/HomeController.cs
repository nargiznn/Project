using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
    public class HomeController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM();
            using (var client = new HttpClient())
            {
                try
                {
                    var eventResponse = await client.GetAsync($"{BaseURl}/api/Event/GetAll");
                    if (eventResponse.IsSuccessStatusCode)
                    {
                        string eventApiResponse = await eventResponse.Content.ReadAsStringAsync();
                        var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(eventApiResponse);

                        homeVM.EventVMs = events.Select(e => new EventVM
                        {
                            Title = e.Title,
                            Desc = e.Desc,
                            ImgUrl = e.ImgUrl,
                            TagsName = e.Tags 
                        });
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + eventResponse.StatusCode;
                        homeVM.EventVMs = new List<EventVM>();
                    }
                    var customerResponse = await client.GetAsync($"{BaseURl}/api/Customer/GetAll");
                    if (customerResponse.IsSuccessStatusCode)
                    {
                        string customerApiResponse = await customerResponse.Content.ReadAsStringAsync();
                        homeVM.Customers = (IEnumerable<Customer>)JsonConvert.DeserializeObject<IEnumerable<Customer>>(customerApiResponse);

                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + customerResponse.StatusCode;
                        homeVM.Customers = new List<Customer>();
                    }


                    var menuCategoryResponse = await client.GetAsync($"{BaseURl}/api/menuCategory/GetAll");
                    if (menuCategoryResponse.IsSuccessStatusCode)
                    {
                        string menuCategoryApiResponse = await menuCategoryResponse.Content.ReadAsStringAsync();
                        homeVM.MenuCategories = (IEnumerable<MenuCategory>)JsonConvert.DeserializeObject<IEnumerable<MenuCategory>>(menuCategoryApiResponse);

                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + menuCategoryResponse.StatusCode;
                        homeVM.MenuCategories = new List<MenuCategory>();
                    }


                    var productResponse = await client.GetAsync($"{BaseURl}/api/Product/GetAll");
                    if (productResponse.IsSuccessStatusCode)
                    {
                        string productApiResponse = await productResponse.Content.ReadAsStringAsync();
                        homeVM.Products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productApiResponse);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + productResponse.StatusCode;
                        homeVM.Products = new List<Product>();
                    }
                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        homeVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }

                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        homeVM.Settings = new Dictionary<string, string>();
                    }

                    //var sliderResponse = await client.GetAsync($"{BaseURl}/api/Slider/GetAll");
                    //if (sliderResponse.IsSuccessStatusCode)
                    //{
                    //    string sliderApiResponse = await sliderResponse.Content.ReadAsStringAsync();
                    //    homeVM.Sliders = (IEnumerable<Slider>)JsonConvert.DeserializeObject<IEnumerable<Slider>>(sliderApiResponse);

                    //}
                    //else
                    //{
                    //    ViewData["Error"] = "API request failed with status code: " + sliderResponse.StatusCode;
                    //    homeVM.Sliders = new List<Slider>();
                    //}

                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failes:{ex.Message}";
                }

              
            }
            return View(homeVM);
        }

    }
}

