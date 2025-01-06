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
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AccountService(UserManager<AppUser> userManager, IMapper mapper,
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
        public async Task<SignUpResponse> SignUpAsync(SignUpDto model)
        {
            var user = _mapper.Map<AppUser>(model);
            var identityResult = await _userManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded)
            {
                return new SignUpResponse { Success = false, Errors = identityResult.Errors.Select(m => m.Description) };
            }

            await _userManager.AddToRoleAsync(user, Roles.SuperAdmin.ToString());
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string confirmUrl = $"{_configuration["ApiBaseUrl"]}/api/account/confirm-email?userId={user.Id}&token={token}";
            string subject = "Please confirm your email address";
            string htmlBody = $"<p>Click the link below to confirm your email:</p><p><a href='{confirmUrl}'>Confirm Email</a></p>";
            await _emailService.SendAsync(user.Email, subject, htmlBody);

            return new SignUpResponse { Success = true, Errors = null };
        }
        public async Task<ConfirmEmailResponse> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ConfirmEmailResponse { Success = false, Errors = new List<string> { "User not found." } };
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return new ConfirmEmailResponse { Success = true };
            }

            return new ConfirmEmailResponse { Success = false, Errors = result.Errors.Select(e => e.Description).ToList() };
        }
        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var mappedDatas = new List<UserDto>();

            foreach (var item in users)
            {
                var userRoles = await _userManager.GetRolesAsync(item);
                var mappedData = _mapper.Map<UserDto>(item);
                mappedData.Roles = userRoles.ToList();
                mappedDatas.Add(mappedData);
            }

            return mappedDatas;
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new NotFoundException(ExceptionMessages.NotFoundMessage);
            return _mapper.Map<UserDto>(user);
        }
        public async Task DeleteUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            foreach (var item in users)
            {
                await _userManager.DeleteAsync(item);
            }
        }

        public async Task DeleteUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new NotFoundException(ExceptionMessages.NotFoundMessage);
            await _userManager.DeleteAsync(user);
        }

        public async Task<IEnumerable<UserDto>> SearchByUserNameAsync(string str)
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users.Where(m => m.UserName.Contains(str)));
        }

        public async Task<IEnumerable<RoleDto>> GetRolesAsync()
        {
            return _mapper.Map<IEnumerable<RoleDto>>(await _roleManager.Roles.ToListAsync());
        }
        public async Task<RoleDto> GetRoleByIdAsync(string id)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(m => m.Id == id);
            if (role == null) throw new NotFoundException(ExceptionMessages.NotFoundMessage);
            return _mapper.Map<RoleDto>(role);
        }
        public async Task<SignInResponse> SignInAsync(SignInDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return new SignInResponse
                {
                    Errors = new List<string> { "Login failed" },
                    Success = false,
                    Token = null
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new SignInResponse
            {
                Success = true,
                Errors = null,
                Token = GenerateJwtToken(user.UserName, roles.ToList())
            };
        }

        private string GenerateJwtToken(string username, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, username)
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


        public async Task AddRoleToUserAsync(UserRoleDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) throw new NotFoundException(ExceptionMessages.NotFoundMessage);

            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null) throw new NotFoundException(ExceptionMessages.NotFoundMessage);

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Contains(role.Name))
            {
                throw new BadRequestException("User already has this role.");
            }

            await _userManager.AddToRoleAsync(user, role.Name);
        }


        //public async Task CreateRoleAsync()
        //{
        //    foreach (var item in Enum.GetValues(typeof(Roles)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(item.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
        //        }
        //    }
        //}
    }
}

