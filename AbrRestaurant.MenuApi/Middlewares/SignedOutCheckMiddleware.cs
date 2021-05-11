using AbrRestaurant.Domain.Errors;
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
                ?.Metadata
                ?.GetMetadata<ControllerActionDescriptor>();

            if (controllerActionDescriptor == null)
            { 
                await next.Invoke(context);
                return;
            }

            var isAuthorizationRequiredEndpoint = 
                controllerActionDescriptor.MethodInfo
                .GetCustomAttributes(false)
                    .Any(p => p is AuthorizeAttribute);

            if (!isAuthorizationRequiredEndpoint)
            {
                await next.Invoke(context);
                return;
            }

            var currentUserFromDb = await _userManager
                .FindByEmailAsync(_currentUser.Email);

            if(currentUserFromDb.LastSignOutMomentTimestamp > _currentUser.IssuedMoment)
            {
                throw AuthenticationRequiredException.UserSignedOutTemplate();
            }
            else
            {
                await next.Invoke(context);
                return;
            }
        }
    }
}
