using AbrRestaurant.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using System.Threading.Tasks;

namespace AbrRestaurant.MenuApi.Middlewares
{
    public class RequestCurrentUserEnrichedMiddleware : IMiddleware
    {
        private readonly ICurrentApplicationUserProvider _currentApplicationUserProvider;
        public RequestCurrentUserEnrichedMiddleware(
            ICurrentApplicationUserProvider currentApplicationUserProvider)
        {
            _currentApplicationUserProvider = currentApplicationUserProvider;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var currentUser = _currentApplicationUserProvider.GetCurrentUser();

            Log.Logger.ForContext("is_current_user_anonymous", currentUser.IsAnonymousUser);
            Log.Logger.ForContext("current_user_id", currentUser.Id);
            Log.Logger.ForContext("current_user_email", currentUser.Email);

            await next.Invoke(context);
            return;
        }
    }
}
