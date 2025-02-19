using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SteakInMCV.Areas.Admin.ViewModels.Event;
using SteakInMCV.Models;
using System.Net.Http;
using Stripe.Forwarding;

namespace SteakInMCV.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {
        private readonly string BaseURL = "http://localhost:7031";
        private readonly HttpClient _httpClient;

        public EventController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        private async Task<T> GetApiResponse<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{BaseURL}/{endpoint}");
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(apiResponse);
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<EventVM> events = await GetApiResponse<IEnumerable<EventVM>>("api/Event/GetAll");
            events = events.OrderByDescending(e => e.Id);
            return View(events);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            EventVM eventVM = await GetApiResponse<EventVM>($"api/Event/getbyid/{id}");
            return View(eventVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseURL}/api/event/delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "An error occurred while deleting the item." });
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var tags = await GetApiResponse<IEnumerable<Tag>>("api/Tag/GetAll");
            var availableTags = tags.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();

            var model = new EventCreateVM
            {
                AvailableTags = availableTags,
                SelectedTags = new List<int>()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateVM model)
        {
            ModelState.Clear();
            var tags = await GetApiResponse<IEnumerable<Tag>>("api/Tag/GetAll");
            var availableTags = tags.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();
            model.AvailableTags = availableTags;

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                TempData["Error"] = "Form məlumatları düzgün daxil edilməyib.";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var multipartContent = new MultipartFormDataContent())
                    {
                        multipartContent.Add(new StringContent(model.Title), "Title");
                        multipartContent.Add(new StringContent(model.Desc), "Desc");
                        multipartContent.Add(new StringContent(model.ImgUrl), "ImgUrl");
                        multipartContent.Add(new StringContent(model.Info), "Info");
                        if (model.SelectedTags != null && model.SelectedTags.Any())
                        {
                            foreach (var tagId in model.SelectedTags)
                            {
                                multipartContent.Add(new StringContent(tagId.ToString()), "TagIds");
                            }
                        }

                        if (model.Image != null)
                        {
                            var fileContent = new StreamContent(model.Image.OpenReadStream());
                            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.Image.ContentType);
                            multipartContent.Add(fileContent, "Image", model.Image.FileName); 
                        }


                        using (var response = await httpClient.PostAsync($"{BaseURL}/api/event/create", multipartContent))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            if (!response.IsSuccessStatusCode)
                            {
                                Console.WriteLine($"API Error: {apiResponse}");
                                ModelState.AddModelError(string.Empty, "API-də xəta baş verdi.");
                                return View(model);
                            }
                        }
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            EventVM eventVM = await GetApiResponse<EventVM>($"api/Event/getbyid/{id}");
            var tags = await GetApiResponse<IEnumerable<Tag>>("api/Tag/GetAll");
            var availableTags = tags.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();

            var model = new EventEditVM
            {
                Id = eventVM.Id,
                Title = eventVM.Title,
                Desc = eventVM.Desc,
                Info = eventVM.Info,
                AvailableTags = availableTags,
                SelectedTags = new List<int>()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventEditVM model)
        {
            ModelState.Clear();
            var tags = await GetApiResponse<IEnumerable<Tag>>("api/Tag/GetAll");
            var availableTags = tags.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();
            model.AvailableTags = availableTags;
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Form məlumatları düzgün daxil edilməyib.";
                return View(model);
            }
            var existingEvent = await GetApiResponse<EventVM>($"api/Event/getbyid/{model.Id}");
            var title = string.IsNullOrEmpty(model.Title) ? existingEvent.Title : model.Title;
            var desc = string.IsNullOrEmpty(model.Desc) ? existingEvent.Desc : model.Desc;
            var info = string.IsNullOrEmpty(model.Info) ? existingEvent.Info : model.Info;
            using (var multipartContent = new MultipartFormDataContent())
            {
                multipartContent.Add(new StringContent(title), "Title");
                multipartContent.Add(new StringContent(desc), "Desc");
                multipartContent.Add(new StringContent(info), "Info");

                if (model.SelectedTags != null && model.SelectedTags.Any())
                {
                    foreach (var tagId in model.SelectedTags)
                    {
                        multipartContent.Add(new StringContent(tagId.ToString()), "TagIds");
                    }
                }

                if (model.Image != null)
                {
                    var fileContent = new StreamContent(model.Image.OpenReadStream());
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.Image.ContentType);
                    multipartContent.Add(fileContent, "Image", model.Image.FileName);
                }
                var response = await _httpClient.PutAsync($"{BaseURL}/api/event/edit/{model.Id}", multipartContent);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Event yenilənərkən xəta baş verdi.";
                    return View(model);
                }
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
