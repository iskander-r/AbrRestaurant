using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Requests;
using AbrRestaurant.MenuApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AbrRestaurant.MenuApi.Controllers.V1
{
    [ApiController]
    public class MenuController : ControllerBase
    {
        public MenuController()
        {
        }


        [HttpGet(ApiRoutesV1.MenuItems.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            throw new NotImplementedException();
        }

        [HttpPost(ApiRoutesV1.MenuItems.Post)]
        public async Task<IActionResult> Post(
            [FromBody] CreateMenuItemRequest model)
        {
            var tempId = Guid.NewGuid();

            var resourceLocation = 
                ResourceLocationProvider.GetLocationUri(
                    ApiRoutesV1.MenuItems.Get, tempId, HttpContext);

            return Created(resourceLocation, null);
        }
    }
}
