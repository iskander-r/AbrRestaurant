using System.Collections.Generic;

namespace AbrRestaurant.Infrastructure.Models
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public bool IsAuthSucceded { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public static AuthenticationResult GetSuccededAuthentication(string token)
        {
            return new AuthenticationResult() { Token = token, IsAuthSucceded = true };
        }

        public static AuthenticationResult GetFailedAuthentication(params string [] errors)
        {
            return new AuthenticationResult() { Errors = errors, Token = null, IsAuthSucceded = false };
        }
    }
}
