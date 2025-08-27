using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniDiscord.GuildService.Data;

namespace MiniDiscord.GuildService.Controllers
{
    [ApiController]
    [Route("api/guilds/{guildId:guid}/members")]
    [Authorize]
    public class MembersController(GuildDbContext db) : ControllerBase
    {
        // El propio usuario se une (join) al guild
        [HttpPost("join")]
        public async Task<IActionResult> Join(Guid guildId)
        {
            var userId = Guid.Parse(User.FindFirst("sub")!.Value);
            var exists = await db.Guilds.AnyAsync(g => g.Id == guildId);
            if (!exists) return NotFound("Guild not found.");

            var already = await db.Memberships.FindAsync(guildId, userId);
            if (already != null) return NoContent();

            db.Memberships.Add(new Membership { GuildId = guildId, UserId = userId, Role = MemberRole.Member });
            await db.SaveChangesAsync();
            return NoContent();
        }
    }

}
