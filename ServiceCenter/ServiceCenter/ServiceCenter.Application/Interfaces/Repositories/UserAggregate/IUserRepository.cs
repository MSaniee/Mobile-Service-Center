using ServiceCenter.Domain.Entities.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenter.Application.Interfaces.Repositories.UserAggregate;

public interface IUserRepository : IRepository<User>
{
    Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
    Task UpdateLastLoginDate(Guid userId, CancellationToken cancellationToken);
    Task<bool> UserExists(string phoneNubmer, CancellationToken cancellationToken);
    Task<User> GetUserByPhoneNumber(string phoneNubmer, CancellationToken cancellationToken);
    Task<User> GetUserById(Guid userId, CancellationToken cancellationToken, bool AsTracking = true);
}
