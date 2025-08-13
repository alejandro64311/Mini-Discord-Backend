
public enum ChannelType { Text = 0, Voice = 1 }
public class Channel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid GuildId { get; set; }
    public string Name { get; set; } = default!;
    public ChannelType Type { get; set; } = ChannelType.Text;

    public Guild Guild { get; set; } = default!;
}