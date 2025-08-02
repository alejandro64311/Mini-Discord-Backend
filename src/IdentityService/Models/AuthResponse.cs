namespace MiniDiscord.IdentityService.Models
{
    public record AuthResponse(string AccessToken, DateTime ExpiresAt);

}
