using AbrRestaurant.Infrastructure.Services;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity.Requests;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AbrRestaurant.MenuApi.Controllers.V1
{
    [ApiController]
    [AllowAnonymous]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        public IdentityController(
            IIdentityService identityService)
        {
            _identityService = identityService;
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
    }
}
