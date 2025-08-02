using MiniDiscord.IdentityService.Models;

namespace MiniDiscord.IdentityService.Services
{
    public interface IJwtGenerator
    {
        AuthResponse Generate(User user);
    }
}
