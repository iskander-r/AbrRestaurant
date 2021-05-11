using AbrRestaurant.MenuApi.Contracts.Shared;
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
    /// <summary>
    /// API-endpoint для ресурса "блюдо в меню"
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    public class MenuControllerV1 : BaseController
    {
        /// <summary>
        /// API возвращает блюдо по ID
        /// </summary>
        [HttpGet(MenuResourceRoutesV1.MenuResource.Get)]
        [ProducesResponseType(typeof(MenuResponseV1), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        public async Task<ActionResult<MenuResponseV1>> Get(
            [FromRoute] GetMenuByIdRequestV1 model)
        {
            var query = model.ToApplicationCommand();
            var response = await _mediator.Send(query);

            return Ok(response.ToOuterContractModel());
        }


        /// <summary>
        /// API возвращает все существующие блюда в меню, с учетом пагинации
        /// </summary>
        [HttpGet(MenuResourceRoutesV1.MenuResource.GetAll)]
        [ProducesResponseType(typeof(MenuResponseV1[]), 200)]
        public async Task<ActionResult<MenuResponseV1[]>> GetAll(
            [FromQuery] GetAllMenuRequestV1 model)
        {
            var query = model.ToApplicationCommand();
            var response = await _mediator.Send(query);

            return Ok(response.Select(p => p.ToOuterContractModel()));
        }


        /// <summary>
        /// API позволяет добавить новое блюдо в меню, требуется аутентификация
        /// </summary>
        [HttpPost(MenuResourceRoutesV1.MenuResource.Post)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(MenuResponseV1), 201)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 401)]
        public async Task<ActionResult<MenuResponseV1>> Post(
            [FromBody] PostMenuRequestV1 model)
        {
            var command = model.ToApplicationCommand();
            var response = await _mediator.Send(command);

            return Ok(response.ToOuterContractModel());
        }


        /// <summary>
        /// API позволяет добавить обновить блюдо в меню по ID, требуется аутентификация
        /// </summary>
        [HttpPut(MenuResourceRoutesV1.MenuResource.Put)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(MenuResponseV1), 201)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 401)]
        public async Task<ActionResult<MenuResponseV1>> Put(
            [FromBody] PutMenuRequestV1 model)
        {
            var command = model.ToApplicationCommand();
            var response = await _mediator.Send(command);

            return Ok(response.ToOuterContractModel());
        }


        /// <summary>
        /// API позволяет удалить блюдо в меню по ID, требуется аутентификация
        /// </summary>
        [HttpDelete(MenuResourceRoutesV1.MenuResource.Get)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        [ProducesResponseType(typeof(ApiErrorResponse), 401)]
        public async Task<ActionResult> Delete(
            [FromRoute] DeleteMenuByIdRequestV1 model)
        {
            var command = model.ToApplicationCommand();
            await _mediator.Send(command);

            return Ok();
        }
    }
}
