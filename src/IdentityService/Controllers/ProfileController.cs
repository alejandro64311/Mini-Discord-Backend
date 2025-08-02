using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniDiscord.IdentityService.Data;
using MiniDiscord.IdentityService.Models;

namespace MiniDiscord.IdentityService.Controllers
{
  

    [ApiController, Route("api/users")]
    [Authorize]
    public class ProfileController(IdentityDbContext db) : ControllerBase
    {
        [HttpGet("me")]
        public async Task<ActionResult<User>> Me()
        {
            Guid id = Guid.Parse(User.FindFirst("sub")!.Value);
            var user = await db.Users.FindAsync(id);
            return user is null ? NotFound() : Ok(user);
        }
    }

}
