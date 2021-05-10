﻿using AbrRestaurant.Infrastructure.Identity;
using AbrRestaurant.Infrastructure.Models;
using AbrRestaurant.Infrastructure.Options;
using AbrRestaurant.Infrastructure.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AbrRestaurant.Infrastructure.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> SignUpAsync(string email, string password);
        Task<AuthenticationResult> SignInAsync(string email, string password);
        Task SignOutAsync();
        Task<ChangePasswordResult> ChangePasswordAsync(string currentPassword, string newPassword);
    }

    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AbrApplicationUser> _userManager;
        private readonly JwtConfigurationOptions _jwtConfigurationOptions;
        private readonly ICurrentApplicationUserProvider _currentUserProvider;
        public IdentityService(
            UserManager<AbrApplicationUser> userManager, 
            JwtConfigurationOptions jwtConfigurationOptions,
            ICurrentApplicationUserProvider currentUserProvider)
        {
            _userManager = userManager;
            _jwtConfigurationOptions = jwtConfigurationOptions;
            _currentUserProvider = currentUserProvider;
        }

 
        public async Task<AuthenticationResult> SignInAsync(
            string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if(existingUser == null)
                return AuthenticationResult.GetFailedAuthentication(
                    $"Указанная комбинация имени пользователя и пароля не найдена!");

            var isValidPassword = await _userManager.CheckPasswordAsync(existingUser, password);

            if(!isValidPassword)
                return AuthenticationResult.GetFailedAuthentication(
                    $"Указанная комбинация имени пользователя и пароля не найдена!");

            return AuthenticateUser(existingUser);
        }


        public async Task<AuthenticationResult> SignUpAsync(
            string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
                return AuthenticationResult.GetFailedAuthentication(
                    $"Пользователь с указанным почтовым адресом '{email}' уже существует!");

            var applicationUser = new AbrApplicationUser()
            {
                Email = email.ToUpper(),
                UserName = email.ToUpper()
            };

            var createdApplicationUser = await _userManager.CreateAsync(applicationUser, password);

            if (!createdApplicationUser.Succeeded)
                return AuthenticationResult.GetFailedAuthentication(
                    "Произошла ошибка при создании пользователя. Пожалуйста, попробуйте позже.");

            return AuthenticateUser(applicationUser);
        }

        private AuthenticationResult AuthenticateUser(
            AbrApplicationUser user)
        {
            // TODO: Decompose to at least 2 classes - JwtGeneratorService, JwtLifetimeService

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfigurationOptions.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("id", user.Id),
                    new Claim("email", user.Email),
                    new Claim("issued_moment", DateTimeExtensions.NowUtcToUnixTimestamp().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return AuthenticationResult.GetSuccededAuthentication(tokenHandler.WriteToken(token));
        }

        public async Task SignOutAsync()
        {
            var applicationUser = await _userManager
                .FindByEmailAsync(_currentUserProvider.GetCurrentUser().Email);

            if (applicationUser == null) return;

            applicationUser.LastSignOutMomentTimestamp = DateTimeExtensions.NowUtcToUnixTimestamp();

            await _userManager.UpdateAsync(applicationUser);
        }

        public async Task<ChangePasswordResult> ChangePasswordAsync(
            string currentPassword, string newPassword)
        {
            var currentUser = _currentUserProvider.GetCurrentUser();
            var applicationUser = await _userManager.FindByEmailAsync(currentUser.Email);

            var changePasswordResult = await _userManager
                .ChangePasswordAsync(applicationUser, currentPassword, newPassword);

            if (!changePasswordResult.Succeeded)
            {
                return ChangePasswordResult.GetFailedPasswordChange(
                    "Не удалось сменить пароль, обратитесь в службу технической поддержки");
            }

            await SignOutAsync();
            return ChangePasswordResult.GetSuccededPasswordChange();
        }
    }
}
