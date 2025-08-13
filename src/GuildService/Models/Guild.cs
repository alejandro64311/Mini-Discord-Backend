public class Guild
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Channel> Channels { get; set; } = new List<Channel>();
    public ICollection<Membership> Members { get; set; } = new List<Membership>();
}