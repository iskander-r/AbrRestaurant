using Microsoft.AspNetCore.Mvc;
using System;

namespace AbrRestaurant.MenuApi.Controllers
{
    [ApiController]
    [Route("api/v1/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(DateTime.Now);
        }
    }
}
