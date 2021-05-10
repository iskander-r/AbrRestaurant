using AbrRestaurant.Infrastructure.Identity;
using AbrRestaurant.Infrastructure.Models;
using AbrRestaurant.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
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
    }

    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfigurationOptions _jwtConfigurationOptions;
        public IdentityService(
            UserManager<IdentityUser> userManager, 
            JwtConfigurationOptions jwtConfigurationOptions)
        {
            _userManager = userManager;
            _jwtConfigurationOptions = jwtConfigurationOptions;
        }


        public async Task<AuthenticationResult> SignInAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthenticationResult> SignUpAsync(string email, string password)
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

            // TODO: Decompose to at least 2 classes - JwtGeneratorService, JwtLifetimeService

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfigurationOptions.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
                    new Claim("Id", applicationUser.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return AuthenticationResult.GetSuccededAuthentication(tokenHandler.WriteToken(token));
        }
    }
}
