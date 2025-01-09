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
                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failed: {ex.Message}";
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

            return View("Client", elementVM);
        }
    }
}

