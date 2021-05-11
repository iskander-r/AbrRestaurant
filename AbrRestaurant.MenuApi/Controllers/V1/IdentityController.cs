using AbrRestaurant.Infrastructure.Services;
using AbrRestaurant.MenuApi.Contracts.Shared;
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
    [Produces("application/json")]
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
        [ProducesResponseType(typeof(CurrentUserProfile), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 401)]
        public async Task<ActionResult<CurrentUserProfile>> GetProfile()
        {
            var profile = await _identityService.GetProfile();
            return Ok(profile);
        }


        /// <summary>
        /// API для регистрации нового пользователя, ROPC-flow
        /// </summary>
        /// <response code="200">Новый пользователь успешно создан</response>
        /// <response code="400">Произошла ошибка при создании пользователя - например, 
        /// необходимо установить более сложный пароль</response>
        [HttpPost(IdentityResourceRoutesV1.IdentityResource.SignUp)]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        public async Task<ActionResult<AuthSuccessResponse>> SignUp(
            [FromBody] UserSignUpRequest model)
        {
            var token = await _identityService.SignUpAsync(model.Email, model.Username, model.Password);       
            
            return Ok(new AuthSuccessResponse(token));
        }


        /// <summary>
        /// API для аутентификации в системе
        /// </summary>
        [HttpPost(IdentityResourceRoutesV1.IdentityResource.SignIn)]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        public async Task<ActionResult<AuthSuccessResponse>> SignIn(
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
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
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
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 401)]
        public async Task<IActionResult> ChangePassword(UserChangePasswordRequest model)
        {
            await _identityService
                .ChangePasswordAsync(model.CurrentPassword, model.NewPassword);

            return Ok();
        }
    }
}
