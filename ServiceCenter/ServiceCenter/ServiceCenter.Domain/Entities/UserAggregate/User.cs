using Microsoft.AspNetCore.Identity;
using ServiceCenter.Domain.Entities.ReceiptAggregate;
using ServiceCenter.Domain.Enums;

namespace ServiceCenter.Domain.Entities.UserAggregate;

public class User : IdentityUser<Guid>, IEntity
{
    public User()
    {
        SecurityStamp = Guid.NewGuid().ToString();
    }

    public string FullName { get; set; }
    public string Avatar { get; set; }
    public string NationalCode { get; set; }
    public string HomeAddress { get; set; }

    public DateTime RegisterDate { get; set; }
    public DateTime? LastLoginDate { get; set; }

    public bool IsActive { get; set; }
    public UserType Type { get; set; }
    public ICollection<Receipt> UserReceipts { get; set; }
    public ICollection<Receipt> TechnicianReceipts { get; set; }
    public ICollection<UserRefreshToken> UserRefreshTokens { get; set; }
}

