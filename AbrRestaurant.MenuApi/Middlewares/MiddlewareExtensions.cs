using Microsoft.AspNetCore.Builder;

namespace AbrRestaurant.MenuApi.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseSignedOutCheckMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SignedOutCheckMiddleware>();
        }


        public static IApplicationBuilder UseRequestCurrentUserEnrichedMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCurrentUserEnrichedMiddleware>();
        }
    }
}
