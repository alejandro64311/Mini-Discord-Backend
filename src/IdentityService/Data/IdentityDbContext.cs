using Microsoft.EntityFrameworkCore;
using MiniDiscord.IdentityService.Models;

namespace MiniDiscord.IdentityService.Data
{
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("users");
                b.HasIndex(x => x.Email).IsUnique();
            });
        }
    }

}
