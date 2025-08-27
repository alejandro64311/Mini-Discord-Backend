namespace MiniDiscord.GuildService.Models
{
    public record CreateGuildDto(string Name);
    public record GuildDto(Guid Id, string Name, DateTime CreatedAt);
    public record JoinGuildDto(Guid UserId); 

}
