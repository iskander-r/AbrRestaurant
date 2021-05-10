using System;

namespace AbrRestaurant.Infrastructure.Utils
{
    public static class Base64Extensions
    {
        public static bool IsValidBase64String(this string base64)
        {
            try
            {
                Convert.FromBase64String(base64);
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
