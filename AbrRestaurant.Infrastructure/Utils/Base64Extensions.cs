using System;

namespace AbrRestaurant.Infrastructure.Utils
{
    public static class Base64Extensions
    {
        public static byte[] ToByteArray(this string base64) => 
            Convert.FromBase64String(base64);
    }
}
