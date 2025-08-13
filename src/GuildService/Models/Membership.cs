public class Membership
{
    public Guid GuildId { get; set; }
    public Guid UserId { get; set; } // viene de Identity (claim "sub")
    public MemberRole Role { get; set; } = MemberRole.Member;

    public Guild Guild { get; set; } = default!;
}

public enum MemberRole { Owner = 0, Admin = 1, Member = 2 }

