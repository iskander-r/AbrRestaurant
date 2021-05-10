using System;

namespace AbrRestaurant.Infrastructure.Utils
{
    public static class DateTimeExtensions
    {
        public static int NowUtcToUnixTimestamp()
        {
            var unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp;
        }
    }
}
