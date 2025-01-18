using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Models;
using SteakInMCV.ViewModels;
using SteakInMCV.ViewModels.Events;
using SteakInMCV.ViewModels.GalleryImage;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInMCV.Controllers
{
    public class AboutController : Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        public async Task<IActionResult> Chef()
        {
            AboutVM aboutVM = new AboutVM();
            using (var client = new HttpClient())
            {
                try
                {
                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        aboutVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        aboutVM.Settings = new Dictionary<string, string>();
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failed: {ex.Message}";
                }


                var chefResponse = await client.GetAsync($"{BaseURl}/api/chef/GetAll");
                if (chefResponse.IsSuccessStatusCode)
                {
                    string chefApiResponse = await chefResponse.Content.ReadAsStringAsync();
                   aboutVM.Chefs = (IEnumerable<Chef>)JsonConvert.DeserializeObject<IEnumerable<Chef>>(chefApiResponse);

                }
                else
                {
                    ViewData["Error"] = "API request failed with status code: " + chefResponse.StatusCode;
                    aboutVM.Chefs = new List<Chef>();
                }

                var productResponse = await client.GetAsync($"{BaseURl}/api/product/GetAll");
                IEnumerable<Product> products = new List<Product>();

                if (productResponse.IsSuccessStatusCode)
                {
                    string productApiResponse = await productResponse.Content.ReadAsStringAsync();
                    products = (IEnumerable<Product>)JsonConvert.DeserializeObject<IEnumerable<Product>>(productApiResponse);
                }

 
                var cuisineResponse = await client.GetAsync($"{BaseURl}/api/cuisine/GetAll");
                if (cuisineResponse.IsSuccessStatusCode)
                {
                    string cuisineApiResponse = await cuisineResponse.Content.ReadAsStringAsync();
                    aboutVM.Cuisines = (IEnumerable<Cuisine>)JsonConvert.DeserializeObject<IEnumerable<Cuisine>>(cuisineApiResponse);
                }
                else
                {
                    ViewData["Error"] = "API request failed with status code: " + cuisineResponse.StatusCode;
                    aboutVM.Cuisines = new List<Cuisine>();
                }
                int totalProducts = products.Count();
                if (totalProducts > 0)
                {
                    foreach (var cuisine in aboutVM.Cuisines)
                    {
                        var productsForCuisine = products.Where(p => p.ProductCuisineName == cuisine.Name).ToList();
                        int productCount = productsForCuisine.Count();
                        if (totalProducts > 0)
                        {
                            cuisine.ProgressPercentage = (int)((double)productCount / totalProducts * 100);
                        }
                    }
                }

                var customerResponse = await client.GetAsync($"{BaseURl}/api/customer/GetAll");
                if (customerResponse.IsSuccessStatusCode)
                {
                    string customerApiResponse = await customerResponse.Content.ReadAsStringAsync();
                    aboutVM.Customers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(customerApiResponse).ToList();
                }
            }

            return View("Chef", aboutVM); 
        }

        public async Task<IActionResult> Story()
        {
            AboutVM aboutVM = new AboutVM();
            using (var client = new HttpClient())
            {
                try
                {
                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        aboutVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        aboutVM.Settings = new Dictionary<string, string>();
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failed: {ex.Message}";
                }

                var eventResponse = await client.GetAsync($"{BaseURl}/api/Event/GetAll");
                if (eventResponse.IsSuccessStatusCode)
                {
                    string eventApiResponse = await eventResponse.Content.ReadAsStringAsync();
                    var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(eventApiResponse);

                    aboutVM.EventVMs = events.Select(e => new EventVM
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
                    aboutVM.EventVMs = new List<EventVM>();
                }

                var customerResponse = await client.GetAsync($"{BaseURl}/api/customer/GetAll");
                if (customerResponse.IsSuccessStatusCode)
                {
                    string customerApiResponse = await customerResponse.Content.ReadAsStringAsync();
                    aboutVM.Customers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(customerApiResponse).ToList();
                }

                var awardResponse = await client.GetAsync($"{BaseURl}/api/award/GetAll");
                if (awardResponse.IsSuccessStatusCode)
                {
                    string awardApiResponse = await awardResponse.Content.ReadAsStringAsync();
                    aboutVM.Awards = JsonConvert.DeserializeObject<List<Award>>(awardApiResponse).ToList(); 
                }
                else
                {
                    ViewData["Error"] = "API request failed with status code: " + awardResponse.StatusCode;
                    aboutVM.Awards = new List<Award>();
                }


                var awardLogoResponse = await client.GetAsync($"{BaseURl}/api/awardlogo/GetAll");
                if (awardLogoResponse.IsSuccessStatusCode)
                {
                    string awardLogoApiResponse = await awardLogoResponse.Content.ReadAsStringAsync();
                    aboutVM.AwardLogos = JsonConvert.DeserializeObject<List<AwardLogo>>(awardLogoApiResponse).ToList();
                }
                else
                {
                    ViewData["Error"] = "API request failed with status code: " + awardLogoResponse.StatusCode;
                    aboutVM.AwardLogos = new List<AwardLogo>();
                }


                var bannerResponse = await client.GetAsync($"{BaseURl}/api/banner/GetAll");
                if (bannerResponse.IsSuccessStatusCode)
                {
                    string bannerApiResponse = await bannerResponse.Content.ReadAsStringAsync();
                    aboutVM.Banners = JsonConvert.DeserializeObject<List<Banner>>(bannerApiResponse).ToList();
                }
                else
                {
                    ViewData["Error"] = "API request failed with status code: " + bannerResponse.StatusCode;
                    aboutVM.Banners = new List<Banner>();
                }

            }

            return View("Story", aboutVM);
        }

        public async Task<IActionResult> Contact()
        {
            AboutVM aboutVM = new AboutVM();
            using (var client = new HttpClient())
            {
                try
                {
                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        aboutVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        aboutVM.Settings = new Dictionary<string, string>();
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failed: {ex.Message}";
                }
            }
            return View(aboutVM);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(AboutVM aboutVM)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        aboutVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        aboutVM.Settings = new Dictionary<string, string>();
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failed: {ex.Message}";
                }
            }

                try
                {
                    var smtpClient = new SmtpClient("smtp.gmail.com") 
                    {
                        Port = 587, 
                        Credentials = new NetworkCredential("nargizzn@code.edu.az", "yswa bxqt nfqf iifz"), 
                        EnableSsl = true
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("your-email@example.com"),
                        Subject = "New Contact Form Submission",
                        Body = $@"
                    Name: {aboutVM.ContactFormModel.FirstName} {aboutVM.ContactFormModel.LastName}
                    Email: {aboutVM.ContactFormModel.Email}
                    Phone: {aboutVM.ContactFormModel.Phone}
                    Message: {aboutVM.ContactFormModel.Message}
                ",
                        IsBodyHtml = false
                    };
                    mailMessage.To.Add("nargizzn@code.edu.az"); 

                    await smtpClient.SendMailAsync(mailMessage);
                    ViewData["Success"] = "Your message has been sent successfully!";
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = $"Failed to send email: {ex.Message}";
                }

            return View("Contact", aboutVM);
        }




        public async Task<IActionResult> Faqs(string searchString = null)
        {
            AboutVM aboutVM = new AboutVM();

            using (var client = new HttpClient())
            {
                try
                {
                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        aboutVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }
                    else
                    {
                        aboutVM.Settings = new Dictionary<string, string>();
                        ViewData["Error"] = "Settings API call failed.";
                    }

                    string faqEndpoint = string.IsNullOrEmpty(searchString)
                        ? $"{BaseURl}/api/faq/GetAll" 
                        : $"{BaseURl}/api/faq/search?searchString={searchString}"; 

                    var faqResponse = await client.GetAsync(faqEndpoint);
                    if (faqResponse.IsSuccessStatusCode)
                    {
                        string faqApiResponse = await faqResponse.Content.ReadAsStringAsync();
                        var faqs = JsonConvert.DeserializeObject<IEnumerable<Faq>>(faqApiResponse);
                        aboutVM.Faqs = faqs.Where(f => f.IsActive).ToList();
                    }
                    else
                    {
                        aboutVM.Faqs = new List<Faq>();
                        ViewData["Error"] = "FAQs API call failed.";
                    }
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = $"An error occurred: {ex.Message}";
                }
            }

            return View("Faqs", aboutVM);
        }


        public async Task<IActionResult> Gallery()
        {
            AboutVM aboutVM = new AboutVM();
            using (var client = new HttpClient())
            {
                try
                {
                    var settingResponse = await client.GetAsync($"{BaseURl}/api/setting/GetAll");
                    if (settingResponse.IsSuccessStatusCode)
                    {
                        string settingApiResponse = await settingResponse.Content.ReadAsStringAsync();
                        var settings = JsonConvert.DeserializeObject<IEnumerable<Setting>>(settingApiResponse);
                        aboutVM.Settings = settings.ToDictionary(s => s.Key, s => s.Value);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + settingResponse.StatusCode;
                        aboutVM.Settings = new Dictionary<string, string>();
                    }

                    var gallerycategoryResponse = await client.GetAsync($"{BaseURl}/api/gallerycategory/GetAll");
                    if (gallerycategoryResponse.IsSuccessStatusCode)
                    {
                        string galleryCategoryApiResponse = await gallerycategoryResponse.Content.ReadAsStringAsync();
                        aboutVM.GalleryCategories = JsonConvert.DeserializeObject<IEnumerable<GalleryCategory>>(galleryCategoryApiResponse);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + gallerycategoryResponse.StatusCode;
                        aboutVM.GalleryCategories = new List<GalleryCategory>();
                    }

                    var galleryImageResponse = await client.GetAsync($"{BaseURl}/api/galleryimage/GetAll");
                    if (galleryImageResponse.IsSuccessStatusCode)
                    {
                        string galleryImageApiResponse = await galleryImageResponse.Content.ReadAsStringAsync();
                        aboutVM.GalleryImagesVM = JsonConvert.DeserializeObject<IEnumerable<GalleryImageVM>>(galleryImageApiResponse);
                    }
                    else
                    {
                        ViewData["Error"] = "API request failed with status code: " + galleryImageResponse.StatusCode;
                        aboutVM.GalleryImagesVM = new List<GalleryImageVM>();
                    }




                }
                catch (HttpRequestException ex)
                {
                    ViewData["Error"] = $"API request failed: {ex.Message}";
                }

            }

            return View("Gallery", aboutVM);
        }

    }
}

