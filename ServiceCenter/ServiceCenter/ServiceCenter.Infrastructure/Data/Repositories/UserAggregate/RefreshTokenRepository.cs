using ServiceCenter.Application.Interfaces.Repositories.UserAggregate;
using ServiceCenter.Common.SecurityTools;
using ServiceCenter.Domain.Core.DILifeTimesType;
using ServiceCenter.Infrastructure.Data.SqlServer.EfCore.Context;
using System.Collections.Generic;

namespace ServiceCenter.Infrastructure.Data.Repositories.UserAggregate;

public class RefreshTokenRepository : Repository<UserRefreshToken>, IRefreshTokenRepository, IScopedDependency
{
    public RefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task AddRefreshTokenAsync(string newRefreshToken, Guid userId, CancellationToken cancellationToken, double expirationDays = 30)
    {
        var now = DateTime.UtcNow;

        UserRefreshToken userRefreshToken = new()
        {
            UserId = userId,
            // Refresh token handles should be treated as secrets and should be stored hashed
            RefreshTokenIdHash = SecurityHelper.GetSha256Hash(newRefreshToken),
            RefreshTokenIdHashSource = newRefreshToken,
            RefreshTokenExpiresDateTime = now.AddDays(expirationDays),
        };

        //await DeleteTokensWithSameRefreshTokenSourceAsync(userRefreshToken.RefreshTokenIdHashSource, cancellationToken);
        //await DeleteTokensWithSameAccessTokenSourceAsync(cancellationToken, personnelId , userId);

        await AddAsync(userRefreshToken, cancellationToken);
    }

    public async Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(refreshTokenIdHashSource))
        {
            return;
        }
        await Table.Where(t => t.RefreshTokenIdHashSource == refreshTokenIdHashSource)
                     .ForEachAsync(userToken =>
                     {
                         DeleteAsync(userToken, cancellationToken);
                     });
    }

    public async Task DeleteTokensWithSameAccessTokenSourceAsync(Guid userId, CancellationToken cancellationToken)
    {
        List<UserRefreshToken> refreshTokens;

        refreshTokens = await Table.Where(t => t.UserId == userId).ToListAsync(cancellationToken);

        await DeleteRangeAsync(refreshTokens, cancellationToken);
    }

    public async Task<string> DeleteTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var userTokens = await Table.Where(c => c.RefreshTokenIdHashSource == refreshToken || c.RefreshTokenIdHash == refreshToken).ToListAsync(cancellationToken);

        if (userTokens.Count <= 0)
            return "NotFound";

        await DeleteRangeAsync(userTokens, cancellationToken);
        return "Ok";
    }

    

    public async Task<string> DeleteTokenAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userTokens = await Table.Where(c => c.UserId == userId).ToListAsync(cancellationToken);

        if (userTokens.Count <= 0)
            return "NotFound";

        await DeleteRangeAsync(userTokens, cancellationToken);
        return "Ok";
    }

    

    public Task<UserRefreshToken> FindTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return null;
        }

        var refreshTokenIdHash = SecurityHelper.GetSha256Hash(refreshToken);
        return TableNoTracking
                .FirstOrDefaultAsync(x => x.RefreshTokenIdHash == refreshTokenIdHash, cancellationToken);
    }
}