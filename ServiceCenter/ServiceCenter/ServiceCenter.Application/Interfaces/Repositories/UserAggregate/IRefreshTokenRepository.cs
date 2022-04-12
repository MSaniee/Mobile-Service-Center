using ServiceCenter.Domain.Entities.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenter.Application.Interfaces.Repositories.UserAggregate;

public interface IRefreshTokenRepository : IRepository<UserRefreshToken>
{
    Task AddRefreshTokenAsync(string newRefreshToken, Guid userId, CancellationToken cancellationToken, double expirationDays = 30);
    Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource, CancellationToken cancellationToken);
    Task DeleteTokensWithSameAccessTokenSourceAsync(Guid userId, CancellationToken cancellationToken);
    Task<string> DeleteTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task<string> DeleteTokenAsync(Guid userId, CancellationToken cancellationToken);
    Task<UserRefreshToken> FindTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
