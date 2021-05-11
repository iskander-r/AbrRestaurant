using AbrRestaurant.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AbrRestaurant.MenuApi.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IMediator _mediator => 
            (IMediator) HttpContext.RequestServices.GetService(typeof(IMediator));
    }
}
