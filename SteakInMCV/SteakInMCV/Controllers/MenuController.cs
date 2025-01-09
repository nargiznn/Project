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
    public class MenuController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Simple()
        {
            MenuVM menuVM = new MenuVM();
            using (var client = new HttpClient())
            {
                try
                {
                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        menuVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }

                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        menuVM.Settings = new Dictionary<string, string>();
                    }
                    var menuCategoryResponse = await client.GetAsync($"{BaseURl}/api/menuCategory/GetAll");
                    if (menuCategoryResponse.IsSuccessStatusCode)
                    {
                        string menuCategoryApiResponse = await menuCategoryResponse.Content.ReadAsStringAsync();
                        var menuCategories = JsonConvert.DeserializeObject<IEnumerable<MenuCategory>>(menuCategoryApiResponse);

                        menuVM.MenuCategories = menuCategories.Select(mc => new MenuCategory
                        {
                            Id = mc.Id,
                            Name = mc.Name,
                            IsActive = mc.IsActive,
                            Products = mc.Products.Select(p => new Product
                            {
                                Name = p.Name,
                                Ingredient = p.Ingredient,
                                Price = p.Price,
                                SpecialCategoryName = p.SpecialCategoryName,
                                FoodCategoryName = p.FoodCategoryName,
                                ProductCuisineName = p.ProductCuisineName,
                                ImageUrls = p.ImageUrls
                            }).ToList()
                        }).ToList();
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + menuCategoryResponse.StatusCode;
                        menuVM.MenuCategories = new List<MenuCategory>();
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failes:{ex.Message}";
                }


            }
            return View("Simple", menuVM);
        }


        public async Task<IActionResult> Classic()
        {
            MenuVM menuVM = new MenuVM();
            using (var client = new HttpClient())
            {
                try
                {
                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        menuVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }

                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        menuVM.Settings = new Dictionary<string, string>();
                    }
                    var menuCategoryResponse = await client.GetAsync($"{BaseURl}/api/menuCategory/GetAll");
                    if (menuCategoryResponse.IsSuccessStatusCode)
                    {
                        string menuCategoryApiResponse = await menuCategoryResponse.Content.ReadAsStringAsync();
                        var menuCategories = JsonConvert.DeserializeObject<IEnumerable<MenuCategory>>(menuCategoryApiResponse);

                        menuVM.MenuCategories = menuCategories.Select(mc => new MenuCategory
                        {
                            Id = mc.Id,
                            Name = mc.Name,
                            IsActive = mc.IsActive,
                            Products = mc.Products.Select(p => new Product
                            {
                                Name = p.Name,
                                Ingredient = p.Ingredient,
                                Price = p.Price,
                                SpecialCategoryName = p.SpecialCategoryName,
                                FoodCategoryName = p.FoodCategoryName,
                                ProductCuisineName = p.ProductCuisineName,
                                ImageUrls = p.ImageUrls
                            }).ToList()
                        }).ToList();
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + menuCategoryResponse.StatusCode;
                        menuVM.MenuCategories = new List<MenuCategory>();
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failes:{ex.Message}";
                }


            }
            return View("Classic", menuVM);
        }
    }
}

