using AbrRestaurant.MenuApi.Contracts.V1;
using AbrRestaurant.MenuApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AbrRestaurant.MenuApi.Controllers.V1
{
    [ApiController]
    public class MenuController : ControllerBase
    {
        [Obsolete("Will be moved to domain handler later. For testing purposes only.")]
        private readonly ApplicationDbContext _applicationDbContext;
        public MenuController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }


        [HttpGet(ApiRoutesV1.MenuItems.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var query = await _applicationDbContext.Meals.ToListAsync();
            return Ok(query);
        }
    }
}
