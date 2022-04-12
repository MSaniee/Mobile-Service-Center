using System.IdentityModel.Tokens.Jwt;

namespace ServiceCenter.Application.Dtos.Tokens;

public class TokenResponse
{
    public AccessToken access_token { get; set; }
    public string refresh_token { get; set; }
}

public class UserTokenResponse
{
    public AccessToken access_token { get; set; }
    public string refresh_token { get; set; }
}

public class AccessToken
{
    public string value { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }

    public AccessToken(JwtSecurityToken securityToken)
    {
        value = new JwtSecurityTokenHandler().WriteToken(securityToken);
        token_type = "Bearer";
        expires_in = (int)(securityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
    }
}


public class UserRefreshTokenDto
{
    public long PersonnelId { get; set; }
    public Guid UserId { get; set; }
    public string NewRefreshToken { get; set; }
}