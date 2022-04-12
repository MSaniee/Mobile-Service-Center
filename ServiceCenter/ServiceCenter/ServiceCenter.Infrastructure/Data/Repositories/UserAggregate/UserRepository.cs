using ServiceCenter.Application.Interfaces.Repositories.UserAggregate;
using ServiceCenter.Domain.Core.DILifeTimesType;
using ServiceCenter.Infrastructure.Data.SqlServer.EfCore.Context;
using System.Collections.Generic;

namespace ServiceCenter.Infrastructure.Data.Repositories.UserAggregate;

public class UserRepository : Repository<User>, IUserRepository, IScopedDependency
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }  

    public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
    {
        user.LastLoginDate = DateTime.Now;
        return UpdateAsync(user, cancellationToken);
    }

    public Task UpdateLastLoginDate(Guid userId, CancellationToken cancellationToken)
    {
        return Table.Where(u => u.Id == userId)
            .UpdateFromQueryAsync(p => new User() { LastLoginDate = DateTime.Now }, cancellationToken);
    }

    public Task<bool> UserExists(string phoneNubmer, CancellationToken cancellationToken)
    {
        return Table.AnyAsync(p => p.PhoneNumber == phoneNubmer, cancellationToken);
    }

    public Task<User> GetUserByPhoneNumber(string phoneNubmer, CancellationToken cancellationToken)
    {
        return Table.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNubmer, cancellationToken);
    }

    public Task<User> GetUserById(Guid userId, CancellationToken cancellationToken, bool AsTracking = true)
    {
        if (AsTracking)
            return Table.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        else
            return TableNoTracking.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    #region

    #endregion

}