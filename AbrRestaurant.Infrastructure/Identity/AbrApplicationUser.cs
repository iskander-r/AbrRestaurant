using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AbrRestaurant.Infrastructure.Identity
{
    public class AbrApplicationUser : IdentityUser
    {
        [Required]
        public int LastSignOutMomentTimestamp { get; set; } = 0;
    }
}
