using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Repository.Exceptions;
using Service.Helpers.DTOs.Account;
using System.Data;
using Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Service.Helpers.Responses;
using Service.Helpers.Enums;
using Service.Helpers;

namespace Service.Services
{
	public class AccountService:IAccountService
	{
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<AppUser> userManager,
                           IMapper mapper,
                           RoleManager<IdentityRole> roleManager,
                           IConfiguration configuration,
                           IEmailService emailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailService = emailService;
        }
        public async Task<SignInResponse> SignInAsync(SignInDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
            if (user is null)
            {
                user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
            }
            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return new SignInResponse
                {
                    Success = false,
                    Errors = new List<string> { "Login failed" },
                    Token = null
                };

            var roles = await _userManager.GetRolesAsync(user);

            return new SignInResponse
            {
                Success = true,
                Errors = null,
                Token = GenerateJwtToken(user.UserName, roles.ToList())
            };
        }
        public async Task AddRoleToUserAsync(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                  ?? throw new NotFoundException(ExceptionMessages.NotFoundMessage);
            var role = await _roleManager.FindByIdAsync(roleId)
                            ?? throw new NotFoundException(ExceptionMessages.NotFoundMessage);
            await _userManager.AddToRoleAsync(user, role.ToString());
        }

        public async Task<VerificationResponse> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId)
                             ?? throw new NotFoundException(ExceptionMessages.NotFoundMessage);
            string decodedToken = Uri.UnescapeDataString(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
            {
                return new VerificationResponse
                {
                    Success = false,
                    Errors = result.Errors.Select(result => result.Description)
                };
            }

            return new VerificationResponse
            {
                Success = true,
                Errors = null
            };
        }

        public async Task CreateRoleAsync()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                }

            }
        }

        public async Task DeleteUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                throw new NotFoundException(ExceptionMessages.NotFoundMessage);

            await _userManager.DeleteAsync(user);
        }

        public async Task DeleteUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequestDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ForgotPasswordResponse
                {
                    Success = false,
                    Errors = new List<string> { "Email address not found." }
                };
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"http://localhost:7031/api/Account/ResetPassword?userId={user.Id}&token={Uri.EscapeDataString(resetToken)}";


            await SendPasswordResetEmailAsync(model.Email, user.UserName, resetLink);

            return new ForgotPasswordResponse
            {
                Success = true,
                Errors = null
            };
        }

        public async Task<RoleDto> GetRoleByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id)
                            ?? throw new NotFoundException(ExceptionMessages.NotFoundMessage);
            return _mapper.Map<RoleDto>(role);
        }

        public async Task<IEnumerable<RoleDto>> GetRolesAsync()
        {
            return _mapper.Map<IEnumerable<RoleDto>>(await _roleManager.Roles.ToListAsync());
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return user == null ? throw new NotFoundException(ExceptionMessages.NotFoundMessage) : _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            return _mapper.Map<IEnumerable<UserDto>>(await _userManager.Users.ToListAsync());
        }

        public async Task RemoveRoleFromUserAsync(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new NotFoundException(ExceptionMessages.NotFoundMessage);
            var role = await _roleManager.FindByIdAsync(roleId)
                            ?? throw new NotFoundException(ExceptionMessages.NotFoundMessage);
            await _userManager.RemoveFromRoleAsync(user, role.ToString());
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequestDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Errors = new List<string> { "User not found." }
                };
            }

            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!resetPasswordResult.Succeeded)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Errors = resetPasswordResult.Errors.Select(e => e.Description).ToList()
                };
            }

            return new ResetPasswordResponse
            {
                Success = true,
                Errors = null
            };
        }

        public async Task<IEnumerable<UserDto>> SearchByUsernameAsync(string str)
        {
            var users = await _userManager.Users.ToListAsync();

            return _mapper.Map<IEnumerable<UserDto>>(users.Where(m => m.UserName.Trim().ToLower().Contains(str.Trim().ToLower())));
        }

        public async Task SendPasswordResetEmailAsync(string email, string username, string resetLink)
        {
            string subject = "Password Reset Request";

            string htmlTemplate;
            using (StreamReader reader = new StreamReader("wwwroot/templates/password_reset.html"))
            {
                htmlTemplate = reader.ReadToEnd();
            }

            htmlTemplate = htmlTemplate.Replace("{{username}}", username);
            htmlTemplate = htmlTemplate.Replace("{{reset-link}}", resetLink);
            htmlTemplate = htmlTemplate.Replace("{{app-name}}", "YourApp");
            htmlTemplate = htmlTemplate.Replace("{{year}}", DateTime.Now.Year.ToString());
            htmlTemplate = htmlTemplate.Replace("{{domain}}", "yourapp.com");

            _emailService.SendEmailAsync(email, subject, htmlTemplate);
        }



        public async Task<SignUpResponse> SignUpAsync(SignUpDto model)
        {
            var user = _mapper.Map<AppUser>(model);
            var identityResponse = await _userManager.CreateAsync(user, model.Password);

            if (!identityResponse.Succeeded)
            {
                return new SignUpResponse
                {
                    Success = false,
                    Errors = identityResponse.Errors.Select(x => x.Description)
                };
            }

            await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = $"http://localhost:7031/api/Account/ConfirmEmail?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            string subject = "Register confirm email";

            string html = string.Empty;

            using (StreamReader reader = new("wwwroot/templates/verification.html"))
            {
                html = reader.ReadToEnd();
            }

            html = html.Replace("{{confirm-link}}", confirmationLink);
            html = html.Replace("{{username}}", model.UserName);
            html = html.Replace("{{dayofweek}}", DateTime.Now.DayOfWeek.ToString());
            html = html.Replace("{{month}}", DateTime.Now.Month.ToString());
            html = html.Replace("{{day}}", DateTime.Now.Day.ToString());
            html = html.Replace("{{hour}}", DateTime.Now.Hour.ToString());


            _emailService.Send(user.Email, subject, html);

            return new SignUpResponse
            {
                Success = true,
                Errors = null
            };
        }


        private string GenerateJwtToken(string username, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, username),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, username)
            };

            roles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

