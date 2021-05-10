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

        protected ActionResult ProcessDomainErrorToApiResponse(DomainError error)
        {
            if(error is DomainValidationFailedError validationError)
                return BadRequest(validationError.Errors);

            throw new InvalidOperationException("Cannot determine domain error type!");
        }
    }
}
