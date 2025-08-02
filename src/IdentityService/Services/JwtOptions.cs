namespace MiniDiscord.IdentityService.Services
{
    public class JwtOptions
    {
        public string Key { get; init; } = default!;
        public string Issuer { get; init; } = default!;
        public int ExpMinutes { get; init; } = 60;
    }

}
