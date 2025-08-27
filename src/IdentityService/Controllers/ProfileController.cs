using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniDiscord.IdentityService.Data;
using MiniDiscord.IdentityService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MiniDiscord.IdentityService.Controllers
{
  

    [ApiController, Route("api/users")]
    [Authorize]
    public class ProfileController(IdentityDbContext db) : ControllerBase
    {
        [HttpGet("me")]
        public async Task<ActionResult<User>> Me()
        {
            var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                     ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid id = Guid.Parse(sub);
            var user = await db.Users.FindAsync(id);
            return user is null ? NotFound() : Ok(user);
        }
    }

}
