namespace ServiceCenter.Website.Dtos;

public class ActivationCodeResponseDto
{
    public bool IsSuccess { get; set; }
    public IEnumerable<string> Errors { get; set; }
}

public class RegistrationResponseDto
{
    public bool IsSuccessfulRegistration { get; set; }
    public IEnumerable<string> Errors { get; set; }
}