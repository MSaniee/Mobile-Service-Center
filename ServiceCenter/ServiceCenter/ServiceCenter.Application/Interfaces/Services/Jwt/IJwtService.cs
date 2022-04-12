using ServiceCenter.Application.Dtos.Tokens;
using ServiceCenter.Domain.Entities.UserAggregate;

namespace ServiceCenter.Application.Interfaces.Services.Jwt;

public interface IJwtService
{
    Task<UserTokenResponse> CreateUserJwtTokens(User user, CancellationToken cancellationToken);
}