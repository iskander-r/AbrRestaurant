using AbrRestaurant.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AbrRestaurant.Infrastructure.Persistence
{
    public class AbrIdentityDbContext : 
        IdentityDbContext<AbrApplicationUser>
    {
        public AbrIdentityDbContext(
            DbContextOptions<AbrIdentityDbContext> options)
            : base(options)
        {
        }
    }
}
