using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniDiscord.IdentityService.Data;
using MiniDiscord.IdentityService.Models;
using MiniDiscord.IdentityService.Services;

namespace MiniDiscord.IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        IdentityDbContext db,
        IJwtGenerator jwt) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await db.Users.AnyAsync(u => u.Email == dto.Email))
                return Conflict("E-mail already registered.");

            var user = new User
            {
                Email = dto.Email.Trim().ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Created(string.Empty, jwt.Generate(user));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            return Ok(jwt.Generate(user));
        }
    }

}
