namespace ServiceCenter.Domain.Entities.UserAggregate;

public class UserRefreshToken : BaseEntity
{
    public string RefreshTokenIdHash { get; set; }
    public string RefreshTokenIdHashSource { get; set; }
    public DateTime RefreshTokenExpiresDateTime { get; set; }
    public bool IsActive { get; set; }

    public Guid? UserId { get; set; }
    public User User { get; set; }
}
