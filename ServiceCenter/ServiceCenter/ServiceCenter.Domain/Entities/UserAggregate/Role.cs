using Microsoft.AspNetCore.Identity;

namespace ServiceCenter.Domain.Entities.UserAggregate;

public class Role : IdentityRole<Guid>, IEntity
{
    public string Description { get; set; }
}