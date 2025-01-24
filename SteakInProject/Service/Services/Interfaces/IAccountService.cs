using System;
using Service.Helpers;
using Service.Helpers.DTOs.Account;
using Service.Helpers.Responses;

namespace Service.Services.Interfaces
{
	public interface IAccountService
	{
        Task<SignUpResponse> SignUpAsync(SignUpDto model);
        Task<VerificationResponse> ConfirmEmail(string userId, string token);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserByIdAsync(string id);
        Task DeleteUsersAsync();
        Task DeleteUserByIdAsync(string id);
        Task<IEnumerable<UserDto>> SearchByUsernameAsync(string str);
        Task CreateRoleAsync();
        Task<IEnumerable<RoleDto>> GetRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(string id);
        Task AddRoleToUserAsync(string userId, string roleId);
        Task RemoveRoleFromUserAsync(string userId, string roleId);
        Task<SignInResponse> SignInAsync(SignInDto model);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequestDto model);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequestDto model);
        Task SendPasswordResetEmailAsync(string email, string username, string resetLink);

    }
}

