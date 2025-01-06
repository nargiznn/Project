using System;
using Service.Helpers;
using Service.Helpers.DTOs.Account;
using Service.Helpers.Responses;

namespace Service.Services.Interfaces
{
	public interface IAccountService
	{
        Task<SignUpResponse> SignUpAsync(SignUpDto model);
        Task<SignInResponse> SignInAsync(SignInDto model);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserByIdAsync(string id);
        Task DeleteUsersAsync();
        Task DeleteUserByIdAsync(string id);
        Task<IEnumerable<UserDto>> SearchByUserNameAsync(string username);
        Task<IEnumerable<RoleDto>> GetRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(string id);
        Task AddRoleToUserAsync(UserRoleDto model);
        Task<ConfirmEmailResponse> ConfirmEmailAsync(string userId, string token);

        //Task CreateRoleAsync();
    }
}

