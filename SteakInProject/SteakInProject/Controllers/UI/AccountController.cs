using System;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.DTOs.Account;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers.UI
{
	public class AccountController:BaseController
	{
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto request)
        {
            var response = await _accountService.SignUpAsync(request);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto request)
        {
            var response = await _accountService.SignInAsync(request);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid email confirmation request.");
            }

            var result = await _accountService.ConfirmEmailAsync(userId, token);
            if (result.Success)
            {
                return Ok("Email successfully confirmed.");
            }

            return BadRequest(result.Errors);
        }

    }
}

