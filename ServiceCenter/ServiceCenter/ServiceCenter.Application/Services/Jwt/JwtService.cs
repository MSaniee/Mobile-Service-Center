using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServiceCenter.Application.Dtos.Tokens;
using ServiceCenter.Application.Interfaces.Repositories.UserAggregate;
using ServiceCenter.Application.Interfaces.Services.Jwt;
using ServiceCenter.Domain.Core.DILifeTimesType;
using ServiceCenter.Domain.Core.Settings.Site;
using ServiceCenter.Domain.Entities.UserAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ServiceCenter.Application.Services.Jwt;

public class JwtService : IJwtService, IScopedDependency
{
    private readonly IRefreshTokenRepository _refreshTokenRepo;
    private readonly SiteSettings _siteSettings;


    public JwtService(
        IOptionsSnapshot<SiteSettings> settings,
        IRefreshTokenRepository refreshTokenRepo
        )
    {
        _siteSettings = settings.Value;
        _refreshTokenRepo = refreshTokenRepo.ThrowIfNull();
    }

    public async Task<UserTokenResponse> CreateUserJwtTokens(User user, CancellationToken cancellationToken)
    {
        string newRefreshToken = GenerateRefreshToken();

        await _refreshTokenRepo.AddRefreshTokenAsync(newRefreshToken, user.Id, cancellationToken).ConfigureAwait(false);

        return new UserTokenResponse
        {
            access_token = GenerateUserAccessTokenAsync(user),
            refresh_token = newRefreshToken
        };
    }

    public AccessToken GenerateUserAccessTokenAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        // authentication successful so generate jwt token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_siteSettings.JwtSettings.SecretKey);
        var encryptionkey = Encoding.UTF8.GetBytes(_siteSettings.JwtSettings.EncryptKey); //must be 16 character
        var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

        var claims = _GetClaims(user);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            NotBefore = DateTime.UtcNow.AddMinutes(0),
            //Expires = DateTime.UtcNow.AddMinutes(_siteSettings.JwtSettings.AccessExpirationMinutes),
            Expires = DateTime.UtcNow.AddDays(30),
            IssuedAt = DateTime.UtcNow,
            Issuer = _siteSettings.JwtSettings.Issuer,
            Audience = _siteSettings.JwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            EncryptingCredentials = encryptingCredentials
        };

        var securityToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        return new AccessToken(securityToken);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private ClaimsIdentity _GetClaims(User user)
    {
        //var result = await _signInManager.ClaimsFactory.CreateAsync(user);
        //var claims = new ClaimsIdentity(result.Claims);

        var claims = new ClaimsIdentity();
        var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

        claims.AddClaims(new[]
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),
            new Claim(ClaimTypes.Role, user.Type.ToString()),
            new Claim(securityStampClaimType,user.SecurityStamp)
        });

        return claims;
    }
}