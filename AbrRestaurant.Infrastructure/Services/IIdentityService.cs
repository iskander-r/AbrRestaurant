using AbrRestaurant.Domain.Errors;
using AbrRestaurant.Infrastructure.Identity;
using AbrRestaurant.Infrastructure.Options;
using AbrRestaurant.Infrastructure.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AbrRestaurant.Infrastructure.Services
{
    public class CurrentUserProfile
    {
        public string Email { get; set; }
        public string Username { get; set; }
    }

    public interface IIdentityService
    {
        Task<string> SignUpAsync(string email, string username, string password);
        Task<string> SignInAsync(string email, string password);
        Task SignOutAsync();
        Task ChangePasswordAsync(string currentPassword, string newPassword);
        Task<CurrentUserProfile> GetProfile();
    }

    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AbrApplicationUser> _userManager;
        private readonly JwtConfigurationOptions _jwtConfigurationOptions;
        private readonly ICurrentApplicationUserProvider _currentUserProvider;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(
            UserManager<AbrApplicationUser> userManager, 
            JwtConfigurationOptions jwtConfigurationOptions,
            ICurrentApplicationUserProvider currentUserProvider,
            ILogger<IdentityService> logger)
        {
            _userManager = userManager;
            _jwtConfigurationOptions = jwtConfigurationOptions;
            _currentUserProvider = currentUserProvider;
            _logger = logger;
        }

 
        public async Task<string> SignInAsync(
            string email, string password)
        {
            var existingUser = await _userManager
                .FindByEmailAsync(email);

            if (existingUser == null)
                throw new BadRequestException(
                    $"Указанная комбинация имени пользователя и пароля не найдена!");

            var isValidPassword = await _userManager
                .CheckPasswordAsync(existingUser, password);

            if(!isValidPassword)
                throw new BadRequestException(
                    $"Указанная комбинация имени пользователя и пароля не найдена!");

            _logger.LogInformation(
                "Пользователь {email} успешно прошел аутентификацию в {currentMoment}", 
                email, DateTime.UtcNow.ToString());

            return AuthenticateUser(existingUser);
        }


        public async Task<string> SignUpAsync(
            string email, string username, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
                throw new BadRequestException(
                    $"Пользователь с указанным почтовым адресом '{email}' уже существует!");

            var applicationUser = new AbrApplicationUser()
            {
                Email = email.ToUpper(),
                UserName = username.ToUpper(),
            };

            var createdApplicationUser = await _userManager
                .CreateAsync(applicationUser, password);

            if (!createdApplicationUser.Succeeded)
                throw new BadRequestException(
                    "Произошла ошибка при создании пользователя. " +
                    "Убедитесь, что ваш пароль состоит как минимум из...");

            _logger.LogInformation(
                "Пользователь {email} успешно зарегистрировался в системе в {currentMoment}",
                email, DateTime.UtcNow.ToString());

            return AuthenticateUser(applicationUser);
        }

        private string AuthenticateUser(
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
            return tokenHandler.WriteToken(token);
        }

        public async Task SignOutAsync()
        {
            var applicationUser = await _userManager
                .FindByEmailAsync(_currentUserProvider.GetCurrentUser().Email);

            if (applicationUser == null) return;

            applicationUser.LastSignOutMomentTimestamp = DateTimeExtensions.NowUtcToUnixTimestamp();

            await _userManager.UpdateAsync(applicationUser);

            var email = applicationUser.Email;
            var currentMoment = DateTime.UtcNow.ToString();

            _logger.LogInformation(
                "Пользователь {email} вышел из системы в {currentMoment}, " +
                "отозвав все свои токены, выпушенные до {currentMoment}",
                email, currentMoment, currentMoment);
        }

        public async Task ChangePasswordAsync(
            string currentPassword, string newPassword)
        {
            var currentUser = _currentUserProvider.GetCurrentUser();
            var applicationUser = await _userManager.FindByEmailAsync(currentUser.Email);

            var changePasswordResult = await _userManager
                .ChangePasswordAsync(applicationUser, currentPassword, newPassword);

            if (!changePasswordResult.Succeeded)
            {
                throw new BadRequestException(
                    "Не удалось сменить пароль, обратитесь в службу технической поддержки");
            }

            await SignOutAsync();

            string email = currentUser.Email;
            var currentMoment = DateTime.UtcNow.ToString();

            _logger.LogInformation(
                "Пользователь {email} сменил свой пароль в {currentMoment}",
                email, currentMoment);
        }

        public async Task<CurrentUserProfile> GetProfile()
        {
            var applicationUser = await _userManager
                .FindByEmailAsync(_currentUserProvider.GetCurrentUser().Email);

            return new CurrentUserProfile
            { 
                Email = applicationUser.Email, Username = applicationUser.UserName
            };
        }
    }
}
