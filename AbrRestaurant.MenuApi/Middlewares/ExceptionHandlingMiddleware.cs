using AbrRestaurant.Domain.Errors;
using AbrRestaurant.Infrastructure.Services;
using AbrRestaurant.MenuApi.Contracts.Shared;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AbrRestaurant.MenuApi.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly CurrentUserIdentity _currentUser;

        public ExceptionHandlingMiddleware(
            ICurrentApplicationUserProvider currentUserProvider)
        {
            _currentUser = currentUserProvider.GetCurrentUser();
        }
        public async Task InvokeAsync(
            HttpContext context, 
            RequestDelegate next)
        {
            var response = default(ApiErrorResponse);

            try
            {
                await next.Invoke(context);
                return;
            }
            catch(BaseException ex)
            {
                response = ApiErrorResponseFactory
                    .CreateFrom(ex, _currentUser.TraceId);
            }
            catch(Exception)
            {
                response = ApiErrorResponseFactory
                    .CreateFrom(new BaseException(), _currentUser.TraceId);        
            }

            context.Response.StatusCode = response.HttpStatusCode;
            context.Response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(response);

            await context.Response.WriteAsync(json);
            return;
        }
    }
}
