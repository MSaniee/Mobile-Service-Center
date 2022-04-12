using ServiceCenter.Application.Dtos.Tokens;
using System.ComponentModel.DataAnnotations;

namespace ServiceCenter.Application.Dtos.Users;

public class UserLoginDto
{
    [Display(Name = nameof(PhoneNumber))]
    [Required(ErrorMessage = "Property is mandatory")]
    [RegularExpression(@"\d{11,14}", ErrorMessage = "Value is not valid")]
    public string PhoneNumber { get; set; }


    [Display(Name = nameof(Code))]
    [Required(ErrorMessage = "Property is mandatory")]
    public int Code { get; set; }
}

public class UserLoginResultDto
{
    public UserTokenResponse Tokens { get; set; }
    public UserInfoDto UserInfo { get; set; }
}

public class UserInfoDto
{
    public string UserName { get; set; }
    public string FullName { get; set; }
}