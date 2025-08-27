using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniDiscord.GuildService.Data;
using MiniDiscord.GuildService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MiniDiscord.GuildService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GuildsController : ControllerBase
    {
        private readonly GuildDbContext _db;
        public GuildsController(GuildDbContext db) => _db = db;

        private Guid CurrentUserId()
        {
            var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                     ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(sub, out var id))
                throw new UnauthorizedAccessException("Token sin 'sub' válido.");
            return id;
        }

        [HttpPost]
        public async Task<ActionResult<GuildDto>> Create([FromBody] CreateGuildDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name es requerido.");

            var userId = CurrentUserId();

            var g = new Guild { Name = dto.Name.Trim() };
            _db.Guilds.Add(g);

            _db.Channels.Add(new Channel
            {
                GuildId = g.Id,
                Name = "general",
                Type = ChannelType.Text
            });

            _db.Memberships.Add(new Membership
            {
                GuildId = g.Id,
                UserId = userId,
                Role = MemberRole.Owner
            });

            await _db.SaveChangesAsync();

            var outDto = new GuildDto(g.Id, g.Name, g.CreatedAt);
            return CreatedAtAction(nameof(GetById), new { id = g.Id }, outDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GuildDto>> GetById(Guid id)
        {
            var g = await _db.Guilds.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (g is null) return NotFound();
            return Ok(new GuildDto(g.Id, g.Name, g.CreatedAt));
        }

        [HttpGet("{id:guid}/channels")]
        public async Task<ActionResult<IEnumerable<object>>> GetChannels(Guid id)
        {
            var exists = await _db.Guilds.AsNoTracking().AnyAsync(x => x.Id == id);
            if (!exists) return NotFound();

            var channels = await _db.Channels.AsNoTracking()
                .Where(c => c.GuildId == id)
                .Select(c => new { c.Id, c.Name, c.Type })
                .ToListAsync();

            return Ok(channels);
        }
    }
}
