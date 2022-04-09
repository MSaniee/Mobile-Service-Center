using System.ComponentModel.DataAnnotations;

namespace ServiceCenter.Application.Dtos.ActivationCodes;

public class SendActivationCodeDto
{
    [Display(Name = nameof(PhoneNumber))]
    [Required(ErrorMessage = "Property is mandatory")]
    [RegularExpression(@"\d{11,14}", ErrorMessage = "تلفن همراه وارد شده صحیح نیست")]
    [StringLength(14, ErrorMessage = "تلفن همراه وارد شده صحیح نیست", MinimumLength = 11)]
    public string PhoneNumber { get; set; }
}

public class SendActivationCodeResultDto
{
    public string CodeExpirationRemainingTime { get; set; }
}