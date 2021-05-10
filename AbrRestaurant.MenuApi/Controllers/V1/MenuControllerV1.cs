using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Mappers;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Requests;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
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
            throw new NotImplementedException();
        }


        [HttpGet(MenuResourceRoutesV1.MenuResource.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            throw new NotImplementedException();
        }

        [HttpPost(MenuResourceRoutesV1.MenuResource.Post)]
        public async Task<IActionResult> Post(
            [FromBody] CreateMenuRequestV1 model)
        {
            var command = model.ToApplicationCommand();
            var commandResponse = await _mediator.Send(command);

            if (commandResponse.CompletedSuccessfully)
                return Ok(commandResponse.Response.ToOuterContractModel());

            return ProcessDomainErrorToApiResponse(commandResponse.Error);
        }
    }
}
