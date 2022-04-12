using ServiceCenter.Application.Interfaces.DataInitializer;
using ServiceCenter.Application.Interfaces.Repositories;
using ServiceCenter.Domain.Entities.UserAggregate;

namespace ServiceCenter.Application.DataInitializer;

public class RolesAndClaimsInitializer : IDataInitializer
{
    public int Order { get; init; } = 1;

    private readonly IRepository<Role> _roleRepo;

    public RolesAndClaimsInitializer(
        IRepository<Role> roleRepo)
    {
        _roleRepo = roleRepo.ThrowIfNull();
    }

    public void InitializeData()
    {
        var roles = new List<Role>
            {
                 new Role() { Name = "GeneralAdmin", NormalizedName = "GENERALADMIN", Description = "مدیر کل"},
                 new Role() { Name = "Admin", NormalizedName = "ADMIN", Description = "ادمین" },
                 new Role() { Name = "User", NormalizedName = "USER", Description = "کاربر" },
            };

        CheckAndAddRoles(roles);
    }

    public void CheckAndAddRoles(List<Role> roles)
    {
        foreach (var role in roles)
        {
            if (!_roleRepo.TableNoTracking.Any(r => r.Name == role.Name))
            {
                _roleRepo.Add(role);
            }
        }
    }
}
