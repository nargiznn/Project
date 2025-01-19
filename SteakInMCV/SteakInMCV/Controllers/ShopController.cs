using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SteakInMCV.Models;
using SteakInMCV.ViewModels;
using SteakInMCV.ViewModels.Events;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Controllers
{
    public class ShopController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Product(int page)
        {
            ShopVM shopVM = new ShopVM();
            using (var client = new HttpClient())
            {
                try
                {

                    var productResponse = await client.GetAsync($"{BaseURl}/api/Product/GetAll");
                    int pageSize = 9;
                    int skip = (page - 1) * pageSize;
                    if (productResponse.IsSuccessStatusCode)
                    {
                        string productApiResponse = await productResponse.Content.ReadAsStringAsync();
                        var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productApiResponse);

                        shopVM.Products = products.Skip(skip).Take(pageSize).ToList();
                        int totalProducts = products.Count();
                        ViewBag.TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
                        ViewBag.PageIndex = page;
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + productResponse.StatusCode;
                        shopVM.Products = new List<Product>();
                    }

                    var foodCategoryResponse = await client.GetAsync($"{BaseURl}/api/foodcategory/GetAll");
                    if (foodCategoryResponse.IsSuccessStatusCode)
                    {
                        string foodCategoryApiResponse = await foodCategoryResponse.Content.ReadAsStringAsync();
                        shopVM.FoodCategories = JsonConvert.DeserializeObject<IEnumerable<FoodCategory>>(foodCategoryApiResponse);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + foodCategoryResponse.StatusCode;
                        shopVM.FoodCategories = new List<FoodCategory>();
                    }

                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        shopVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }

                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        shopVM.Settings = new Dictionary<string, string>();
                    }

                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failes:{ex.Message}";
                }


            }
            return View("Product", shopVM);
        }


        public async Task<IActionResult> Single(int id)
        {
            ShopVM shopVM = new ShopVM();
            using (var client = new HttpClient())
            {
                try
                {
                    var tagResponse = await client.GetAsync($"{BaseURl}/api/tag/GetAll");
                    if (tagResponse.IsSuccessStatusCode)
                    {
                        string tagApiResponse = await tagResponse.Content.ReadAsStringAsync();
                        shopVM.Tags = (IEnumerable<Tag>)JsonConvert.DeserializeObject<IEnumerable<Tag>>(tagApiResponse);

                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + tagResponse.StatusCode;
                        shopVM.Tags = new List<Tag>();
                    }

                    var productAllResponse = await client.GetAsync($"{BaseURl}/api/Product/GetAll");
                    if (productAllResponse.IsSuccessStatusCode)
                    {
                        string productAllApiResponse = await productAllResponse.Content.ReadAsStringAsync();
                        shopVM.Products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productAllApiResponse);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + productAllResponse.StatusCode;
                        shopVM.Products = new List<Product>();
                    }

                    var productResponse = await client.GetAsync($"{BaseURl}/api/product/getbyid/{id}");
                    if (productResponse.IsSuccessStatusCode)
                    {
                        string productApiResponse = await productResponse.Content.ReadAsStringAsync();
                        shopVM.Product = JsonConvert.DeserializeObject<Product>(productApiResponse);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + productResponse.StatusCode;
                        shopVM.Product = null;
                    }

                    var foodCategoryResponse = await client.GetAsync($"{BaseURl}/api/foodcategory/GetAll");
                    if (foodCategoryResponse.IsSuccessStatusCode)
                    {
                        string foodCategoryApiResponse = await foodCategoryResponse.Content.ReadAsStringAsync();
                        shopVM.FoodCategories = JsonConvert.DeserializeObject<IEnumerable<FoodCategory>>(foodCategoryApiResponse);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + foodCategoryResponse.StatusCode;
                        shopVM.FoodCategories = new List<FoodCategory>();
                    }

                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        shopVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }

                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        shopVM.Settings = new Dictionary<string, string>();
                    }

                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failes:{ex.Message}";
                }


            }
            return View("Single", shopVM);
        }
    }
}

