using AbrRestaurant.Infrastructure.Identity;
using AbrRestaurant.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AbrRestaurant.MenuApi.Middlewares
{
    public class SignedOutCheckMiddleware : IMiddleware
    {
        private readonly CurrentUserIdentity _currentUser;
        private readonly UserManager<AbrApplicationUser> _userManager;
        public SignedOutCheckMiddleware(
            ICurrentApplicationUserProvider currentUserProvider,
            UserManager<AbrApplicationUser> userManager)
        {
            _currentUser = currentUserProvider.GetCurrentUser();
            _userManager = userManager;
        }


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // TODO: apply some caching policy aka ConcurrentDictionary,
            // as reflection is not the best idea for every request middleware

            var controllerActionDescriptor = context
                .GetEndpoint()
                .Metadata
                .GetMetadata<ControllerActionDescriptor>();

            var isAuthorizationRequiredEndpoint = 
                controllerActionDescriptor.MethodInfo.GetCustomAttributes(false)
                .Any(p => p is AuthorizeAttribute);

            if (!isAuthorizationRequiredEndpoint)
            {
                await next.Invoke(context);
            }

            if(_currentUser.IsAnonymousUser)
            {
                CancelProcessingPipelineAs401(context);
            }

            var currentUserFromDb = await _userManager
                .FindByEmailAsync(_currentUser.Email);

            if(currentUserFromDb.LastSignOutMomentTimestamp != null && 
                currentUserFromDb.LastSignOutMomentTimestamp > _currentUser.IssuedMoment)
            {
                CancelProcessingPipelineAs401(context);
            }
            else
            {
                await next.Invoke(context);
            }
        }

        private void CancelProcessingPipelineAs401(HttpContext context)
        {
            context.Response.StatusCode = 401;
        }
    }
}
