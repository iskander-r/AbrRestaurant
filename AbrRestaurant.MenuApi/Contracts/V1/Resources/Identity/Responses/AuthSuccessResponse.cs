﻿namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity.Responses
{
    public class AuthSuccessResponse
    {
        public string Token { get; set; }

        public AuthSuccessResponse(string token)
        {
            Token = token;
        }
    }
}
