using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SteakInMCV.Models;
using SteakInMCV.ViewModels;
using SteakInMCV.ViewModels.Events;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Controllers
{
    public class BlogController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Standart(int page=1)
        {
            BlogVM blogVM = new BlogVM();
            using (var client = new HttpClient())
            {
                try
                {
                    var eventResponse = await client.GetAsync($"{BaseURl}/api/Event/GetAll");
                    if (eventResponse.IsSuccessStatusCode)
                    {
                        string eventApiResponse = await eventResponse.Content.ReadAsStringAsync();
                        var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(eventApiResponse);
                        int pageSize = 3; 
                        int skip = (page - 1) * pageSize;
                        blogVM.EventVMs = events
                            .Select(e => new EventVM
                            {
                                Title = e.Title,
                                Desc = e.Desc,
                                ImgUrl = e.ImgUrl,
                                Info = e.Info,
                                TagsName = e.Tags
                            })
                            .Skip(skip)
                            .Take(pageSize)
                            .ToList();

                        int totalEvents = events.Count();
                        ViewBag.TotalPages = (int)Math.Ceiling(totalEvents / (double)pageSize);  
                        ViewBag.PageIndex = page;
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + eventResponse.StatusCode;
                        blogVM.EventVMs = new List<EventVM>();
                    }
                    //var customerResponse = await client.GetAsync($"{BaseURl}/api/Customer/GetAll");
                    //if (customerResponse.IsSuccessStatusCode)
                    //{
                    //    string customerApiResponse = await customerResponse.Content.ReadAsStringAsync();
                    //    blogVM.Customers = (IEnumerable<Customer>)JsonConvert.DeserializeObject<IEnumerable<Customer>>(customerApiResponse);

                    //}
                    //else
                    //{
                    //    ViewData["Error"] = "API request failed with status code: " + customerResponse.StatusCode;
                    //    blogVM.Customers = new List<Customer>();
                    //}
                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        blogVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }

                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        blogVM.Settings = new Dictionary<string, string>();
                    }


                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failes:{ex.Message}";
                }


            }
            return View("Standart", blogVM);
        }


        public async Task<IActionResult> Single(int id, int page = 1)
        {
            BlogVM blogVM = new BlogVM();
            using (var client = new HttpClient())
            {
                try
                {
                    var eventResponse = await client.GetAsync($"{BaseURl}/api/Event/GetAll");
                    if (eventResponse.IsSuccessStatusCode)
                    {
                        string eventApiResponse = await eventResponse.Content.ReadAsStringAsync();
                        var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(eventApiResponse);
                        int pageSize = 3;
                        int skip = (page - 1) * pageSize;
                        blogVM.EventVMs = events
                            .Select(e => new EventVM
                            {
                                Title = e.Title,
                                Desc = e.Desc,
                                ImgUrl = e.ImgUrl,
                                Info = e.Info,
                                TagsName = e.Tags
                            })
                            .Skip(skip)
                            .Take(pageSize)
                            .ToList();

                        int totalEvents = events.Count();
                        ViewBag.TotalPages = (int)Math.Ceiling(totalEvents / (double)pageSize);
                        ViewBag.PageIndex = page;
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + eventResponse.StatusCode;
                        blogVM.EventVMs = new List<EventVM>();
                    }

                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        blogVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        blogVM.Settings = new Dictionary<string, string>();
                    }
                    var eventSingleResponse = await client.GetAsync($"{BaseURl}/api/event/getbyid/{id}");
                    if (eventSingleResponse.IsSuccessStatusCode)
                    {
                        string eventSingleApiResponse = await eventSingleResponse.Content.ReadAsStringAsync();
                        var eventSingle = JsonConvert.DeserializeObject<Event>(eventSingleApiResponse);


                        blogVM.EventVM = new EventVM
                        {
                            Title = eventSingle.Title,
                            Desc = eventSingle.Desc,
                            ImgUrl = eventSingle.ImgUrl,
                            Info = eventSingle.Info,
                            TagsName = eventSingle.Tags,
                            CreatedDate = eventSingle.CreatedDate
                        };
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + eventSingleResponse.StatusCode;
                        blogVM.EventVM = null;
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failed: {ex.Message}";
                }
            }
            return View("Single", blogVM);
        }

    }
}

