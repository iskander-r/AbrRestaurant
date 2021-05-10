using AbrRestaurant.Infrastructure.Services;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity.Requests;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AbrRestaurant.MenuApi.Controllers.V1
{
    [ApiController]
    [AllowAnonymous]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(
            IIdentityService identityService,
            ILogger<IdentityController> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }


        [HttpPost(IdentityResourceRoutesV1.IdentityResource.SignUp)]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpRequest model)
        {
            var authResposne = await _identityService.SignUpAsync(model.Email, model.Password);

            if (!authResposne.IsAuthSucceded)
                return BadRequest(new AuthFailedResponse(authResposne.Errors));
            
            return Ok(new AuthSuccessResponse { Token = authResposne.Token });
        }


        [HttpPost(IdentityResourceRoutesV1.IdentityResource.SignIn)]
        public async Task<IActionResult> SignIn([FromBody] UserSignInRequest model)
        {
            var authResposne = await _identityService.SignInAsync(model.Email, model.Password);

            if (!authResposne.IsAuthSucceded)
                return BadRequest(new AuthFailedResponse(authResposne.Errors));

            return Ok(new AuthSuccessResponse { Token = authResposne.Token });
        }


        [HttpPost(IdentityResourceRoutesV1.IdentityResource.SignOut)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public new async Task<IActionResult> SignOut()
        {
            await _identityService.SignOutAsync();
            return Ok();
        }


        [HttpPost(IdentityResourceRoutesV1.IdentityResource.ChangePassword)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(UserChangePasswordRequest model)
        {
            var result = await _identityService
                .ChangePasswordAsync(model.CurrentPassword, model.NewPassword);

            return Ok(result);
        }
    }
}
