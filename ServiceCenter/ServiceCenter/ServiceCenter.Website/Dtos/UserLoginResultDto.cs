using ServiceCenter.Application.Dtos.Users;

namespace ServiceCenter.Website.Dtos;

public class LoginResultDto
{
    public TokenResponse Tokens { get; set; }
    public UserInfoDto UserInfo { get; set; }
}

public class TokenResponse
{
    public AccessTokenResult access_token { get; set; }
    public string refresh_token { get; set; }
}

public class AccessTokenResult
{
    public string value { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
}