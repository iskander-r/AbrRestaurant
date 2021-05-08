using Microsoft.AspNetCore.Http;

namespace AbrRestaurant.MenuApi.Helpers
{
    public static class ResourceLocationProvider
    {
        public static string GetLocationUri(
            string resourceGetPath, 
            object resourceIdentifier,
            HttpContext httpContext)
        {
            var baseUri = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.ToUriComponent()}";
            var locationUri = $"{baseUri}/{resourceGetPath.Replace("{id}", resourceIdentifier.ToString())}";

            return locationUri;
        }
    }
}
