using System;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.DTOs.Account;
using Service.Helpers.Responses;
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

        [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto request)
        {
            var response = await _accountService.SignUpAsync(request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [ProducesResponseType(typeof(VerificationResponse), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var result = await _accountService.ConfirmEmail(userId, token);
            return Ok(result);
        }

        [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInDto request)
        {
            var response = await _accountService.SignInAsync(request);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto model)
        {
            var response = await _accountService.ForgotPasswordAsync(model);

            if (!response.Success)
            {
                return BadRequest(response.Errors);
            }

            return Ok("Password reset link has been sent to your email.");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto model)
        {
            var response = await _accountService.ResetPasswordAsync(model);

            if (!response.Success)
            {
                return BadRequest(response.Errors);
            }

            return Ok("Your password has been reset successfully.");
        }
    }
}

