using Microsoft.EntityFrameworkCore;

namespace MiniDiscord.GuildService.Data
{
    public class GuildDbContext(DbContextOptions<GuildDbContext> o) : DbContext(o)
    {
        public DbSet<Guild> Guilds => Set<Guild>();
        public DbSet<Channel> Channels => Set<Channel>();
        public DbSet<Membership> Memberships => Set<Membership>();
        protected override void OnModelCreating(ModelBuilder model)
        {
            model.HasDefaultSchema("guild");

            model.Entity<Guild>(b =>
            {
                b.ToTable("guilds");
                b.Property(x => x.Name).HasMaxLength(100).IsRequired();
                b.HasIndex(x => x.Name);
            });

            model.Entity<Channel>(b =>
            {
                b.ToTable("channels");
                b.Property(x => x.Name).HasMaxLength(100).IsRequired();
                b.HasOne(x => x.Guild).WithMany(g => g.Channels).HasForeignKey(x => x.GuildId);
                b.HasIndex(x => new { x.GuildId, x.Name }).IsUnique(); 
            });

            model.Entity<Membership>(b =>
            {
                b.ToTable("memberships");
                b.HasKey(x => new { x.GuildId, x.UserId });
                b.Property(x => x.Role).HasConversion<int>();
                b.HasOne(x => x.Guild).WithMany(g => g.Members).HasForeignKey(x => x.GuildId);
            });
        }
    }

}
