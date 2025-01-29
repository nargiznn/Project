using System;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteakInMCV.Helpers.Response;
using SteakInMCV.ViewModels.Account;

namespace SteakInMCV.Controllers
{
	public class AccountController: Controller
    {
        private readonly string BaseURl = "http://localhost:7031";
        private readonly IHttpContextAccessor _httpContext;

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public AccountController(IHttpClientFactory httpClientFactory, HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClientFactory.CreateClient("SteakInMCV");
            _httpClient.BaseAddress = new Uri(configuration["BaseUrl"]);
            _httpContext = httpContext;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            var jsonData = JsonConvert.SerializeObject(registerVM);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PostAsync($"{BaseURl}/api/Account/SignUp", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("RegisterConfirmation");
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                ModelState.AddModelError("UserName", "Username already exists. Please choose a different username.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred while registering the account.");
            }

            return View(registerVM);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseURl}/api/Account/SignIn", model);
                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResponse = System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(responseContent, options);

                if (loginResponse.Success)
                {

                    HttpContext.Session.SetString("AuthToken", loginResponse.Token);


                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Login failed");
                    return View();

                }
            }
            catch (Exception ex)
            {

                ViewData["Error"] = "An unexpected error occurred. Please try again.";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult RegisterConfirmation()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AuthToken");


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Email cannot be empty.");
                return View();
            }

            var requestUri = $"{BaseURl}/api/Account/ForgotPassword";
            var response = await _httpClient.PostAsJsonAsync(requestUri, email);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObj = System.Text.Json.JsonSerializer.Deserialize<ResponseObj>(responseContent, options);
                TempData["Message"] = responseObj.ResponseMessage;
                return RedirectToAction("ForgetPasswordConfirmation");
            }
            else
            {
                var responseObj = await response.Content.ReadFromJsonAsync<ResponseObj>();
                ModelState.AddModelError("", responseObj.ResponseMessage);
                return View();
            }
        }

        public IActionResult ForgetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {

            var model = new UserPasswordVM
            {
                email = email,
                token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(UserPasswordVM userPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(userPasswordVM);
            }

            var requestUri = $"{BaseURl}/api/Account/ResetPassword";
            var response = await _httpClient.PostAsJsonAsync(requestUri, userPasswordVM);

            if (response.IsSuccessStatusCode)
            {
                var responseObj = await response.Content.ReadFromJsonAsync<ResponseObj>();
                TempData["Message"] = responseObj.ResponseMessage;
                return RedirectToAction("ResetPasswordConfirmation");
            }
            else
            {
                var responseObj = await response.Content.ReadFromJsonAsync<ResponseObj>();
                ModelState.AddModelError("", responseObj.ResponseMessage);
                return View(userPasswordVM);
            }
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}

