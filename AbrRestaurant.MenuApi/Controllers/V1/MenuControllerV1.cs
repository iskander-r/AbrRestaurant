using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Requests;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Responses;
using AbrRestaurant.MenuApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AbrRestaurant.MenuApi.Controllers.V1
{
    [ApiController]
    public class MenuControllerV1 : ControllerBase
    {
        public MenuControllerV1()
        {
        }

        [HttpGet(MenuResourceRoutesV1.MenuResource.Get)]
        public async Task<ActionResult<MenuResponseV1>> Get(
            [FromRoute] GetMenuByIdRequestV1 model)
        {
            var resource = new MenuResponseV1()
            {
                Id = model.Id,
                Name = string.Empty,
                Description = string.Empty,
                PictureUri = string.Empty,
                Price = 0
            };

            return Ok(resource);
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
            var tempId = Guid.NewGuid();

            var resource = new MenuResponseV1()
            { 
                Id = tempId.ToString(),
                Name = string.Empty, 
                Description = string.Empty, 
                PictureUri = string.Empty, Price = 0 
            };

            var resourceLocation = 
                ResourceLocationProvider.GetLocationUri(
                    MenuResourceRoutesV1.MenuResource.Get, tempId, HttpContext);

            return Created(resourceLocation, resource);
        }
    }
}
