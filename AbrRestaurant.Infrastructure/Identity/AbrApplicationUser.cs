using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace AbrRestaurant.Infrastructure.Identity
{
    public class AbrApplicationUser : IdentityUser
    {
        [Required]
        public string Username { get; set; }
        public int ? LastSignOutMomentTimestamp { get; set; }
    }
}
