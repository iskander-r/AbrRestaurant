using System.Collections.Generic;

namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity.Responses
{
    public class AuthFailedResponse
    {
        public AuthFailedResponse(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public IEnumerable<string> Errors { get; set; }
    }
}
