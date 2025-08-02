using Microsoft.EntityFrameworkCore;
using MiniDiscord.IdentityService.Models;

namespace MiniDiscord.IdentityService.Data
{
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
    }

}
