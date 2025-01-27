using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using System.Data;
using Service.Helpers.DTOs.Account;

namespace SteakInProject.Controllers.Admin
{
    //[Authorize(Roles = "SuperAdmin")]
    public class AccountController : MainController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [HttpGet]

        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _accountService.GetUsersAsync());
        }

        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]

        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            return Ok(await _accountService.GetUserByIdAsync(id));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete]
        public async Task<IActionResult> DeleteUsers()
        {
            await _accountService.DeleteUsersAsync();
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> DeleteUserById([FromRoute] string id)
        {
            await _accountService.DeleteUserByIdAsync(id);
            return Ok();
        }

        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> SearchByUsername([FromQuery] string searchText)
        {
            return Ok(await _accountService.SearchByUsernameAsync(searchText));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> CreateRole()
        {
            await _accountService.CreateRoleAsync();
            return Ok();
        }

        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _accountService.GetRolesAsync());
        }
        
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById([FromRoute] string id)
        {
            return Ok(await _accountService.GetRoleByIdAsync(id));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<IActionResult> AddRoleToUser([FromQuery] string userId, [FromQuery] string roleId)
        {
            await _accountService.AddRoleToUserAsync(userId, roleId);
            return Ok();
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<IActionResult> RemoveRoleFromUser([FromQuery] string userId, [FromQuery] string roleId)
        {
            await _accountService.RemoveRoleFromUserAsync(userId, roleId);
            return Ok();
        }

    }
}

