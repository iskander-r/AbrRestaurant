using AbrRestaurant.Infrastructure.Services;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity.Requests;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AbrRestaurant.MenuApi.Controllers.V1
{
    /// <summary>
    /// API-endpoint-ы для для управления пользователем и сессией
    /// </summary>
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(
            IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// API возвращает профиль текущего пользователя. Требуется аутентификация.
        /// </summary>
        [HttpGet(IdentityResourceRoutesV1.IdentityResource.GetProfile)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _identityService.GetProfile();
            return Ok(profile);
        }


        /// <summary>
        /// API для регистрации нового пользователя, ROPC-flow
        /// </summary>
        [HttpPost(IdentityResourceRoutesV1.IdentityResource.SignUp)]
        public async Task<IActionResult> SignUp(
            [FromBody] UserSignUpRequest model)
        {
            var token = await _identityService.SignUpAsync(model.Email, model.Username, model.Password);       
            
            return Ok(new AuthSuccessResponse(token));
        }


        /// <summary>
        /// API для аутентификации в системе
        /// </summary>
        [HttpPost(IdentityResourceRoutesV1.IdentityResource.SignIn)]
        public async Task<IActionResult> SignIn(
            [FromBody] UserSignInRequest model)
        {
            var token = await _identityService.SignInAsync(model.Email, model.Password);
            
            return Ok(new AuthSuccessResponse(token));
        }


        /// <summary>
        /// API отзывает и делает невалидными все токены, выпущенные ДО текущего момента. 
        /// Требуется аутентификация
        /// </summary>
        [HttpPost(IdentityResourceRoutesV1.IdentityResource.SignOut)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public new async Task<IActionResult> SignOut()
        {
            await _identityService.SignOutAsync();
            
            return Ok();
        }


        /// <summary>
        /// API позволяет сменить пароль текущего пользователя. 
        /// Неявно вызывается sign_out.  Требуется аутентификация
        /// </summary>
        [HttpPost(IdentityResourceRoutesV1.IdentityResource.ChangePassword)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(UserChangePasswordRequest model)
        {
            await _identityService
                .ChangePasswordAsync(model.CurrentPassword, model.NewPassword);

            return Ok();
        }
    }
}
