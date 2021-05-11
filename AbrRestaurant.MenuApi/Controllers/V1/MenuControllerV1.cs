using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Mappers;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Requests;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;


namespace AbrRestaurant.MenuApi.Controllers.V1
{
    [ApiController]
    public class MenuControllerV1 : BaseController
    {
        [HttpGet(MenuResourceRoutesV1.MenuResource.Get)]
        public async Task<ActionResult<MenuResponseV1>> Get(
            [FromRoute] GetMenuByIdRequestV1 model)
        {
            var query = model.ToApplicationCommand();
            var response = await _mediator.Send(query);

            return Ok(response.ToOuterContractModel());
        }


        [HttpGet(MenuResourceRoutesV1.MenuResource.GetAll)]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetAllMenuRequestV1 model)
        {
            var query = model.ToApplicationCommand();
            var response = await _mediator.Send(query);

            return Ok(response.Select(p => p.ToOuterContractModel()));
        }


        [HttpPost(MenuResourceRoutesV1.MenuResource.Post)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post(
            [FromBody] PostMenuRequestV1 model)
        {
            var command = model.ToApplicationCommand();
            var response = await _mediator.Send(command);

            return Ok(response.ToOuterContractModel());
        }


        [HttpPut(MenuResourceRoutesV1.MenuResource.Put)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Put(
            [FromBody] PutMenuRequestV1 model)
        {
            var command = model.ToApplicationCommand();
            var response = await _mediator.Send(command);

            return Ok(response.ToOuterContractModel());
        }


        [HttpDelete(MenuResourceRoutesV1.MenuResource.Get)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(
            [FromRoute] DeleteMenuByIdRequestV1 model)
        {
            var command = model.ToApplicationCommand();
            await _mediator.Send(command);

            return Ok();
        }
    }
}
