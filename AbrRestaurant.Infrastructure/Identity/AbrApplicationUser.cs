using Microsoft.AspNetCore.Identity;
using System;

namespace AbrRestaurant.Infrastructure.Identity
{
    public class AbrApplicationUser : IdentityUser
    {
        public int ? LastSignOutMomentTimestamp { get; set; }
    }
}
