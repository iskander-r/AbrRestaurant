using AbrRestaurant.Domain.Errors;
using AbrRestaurant.Infrastructure.Services;
using AbrRestaurant.MenuApi.Contracts.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AbrRestaurant.MenuApi.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly CurrentUserIdentity _currentUser;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            ICurrentApplicationUserProvider currentUserProvider,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _currentUser = currentUserProvider.GetCurrentUser();
            _logger = logger;
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

                var currentMoment = response.MomentUtc;
                var traceId = response.TraceId;
                var exceptionType = ex.GetType().Name;

                _logger.LogError(
                    "Произошла обработанная ошибка доменного уровня в {currentMoment}, " +
                    "traceId = {traceId}, тип ошибки = {exceptionType},",
                    currentMoment, traceId, exceptionType);
            }
            catch(Exception ex)
            {
                response = ApiErrorResponseFactory
                    .CreateFrom(new BaseException(), _currentUser.TraceId);

                var currentMoment = DateTime.UtcNow.ToString();
                var traceId = _currentUser.TraceId;
                var exceptionType = ex.GetType().Name;
                var message = ex.Message;

                _logger.LogCritical(
                    "Произошла необработнная ошибка в {currentMoment}, " +
                    "traceId = {traceId}, тип ошибки = {exceptionType}, тело ошибки = {message}",
                    currentMoment, traceId, exceptionType, message);
            }

            context.Response.StatusCode = response.HttpStatusCode;
            context.Response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(response);

            await context.Response.WriteAsync(json);
            return;
        }
    }
}
