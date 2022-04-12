using MediatR;
using ServiceCenter.Application.Dtos.Users;
using ServiceCenter.Application.Interfaces.Repositories.UserAggregate;
using ServiceCenter.Application.Interfaces.Services.Jwt;
using ServiceCenter.Common.Resources;
using ServiceCenter.Domain.Entities.UserAggregate;
using ServiceCenter.Domain.Enums;

namespace ServiceCenter.Application.Features.UserAggregate.Users;

public record RegisterOrLoginUserCommand(
    string PhoneNumber,
    int Code)
    : IRequest<SResult<UserLoginResultDto>>;

public class RegisterOrLoginUserCommandHandler : IRequestHandler<RegisterOrLoginUserCommand, SResult<UserLoginResultDto>>
{
    private readonly IActivationCodeRepository _activationCodeRepo;
    private readonly IUserRepository _userRepo;
    private readonly IJwtService _jwtService;

    public RegisterOrLoginUserCommandHandler(
        IActivationCodeRepository activationCodeRepo,
        IUserRepository userRepo,
        IJwtService jwtService)
    {
        _activationCodeRepo = activationCodeRepo.ThrowIfNull();
        _userRepo = userRepo.ThrowIfNull();
        _jwtService = jwtService.ThrowIfNull();
    }

    public async Task<SResult<UserLoginResultDto>> Handle(RegisterOrLoginUserCommand request, CancellationToken cancellationToken)
    {
        UserLoginResultDto result = new();
        result.UserInfo = new();
        DateTime dtNow = DateTime.Now;

        ActivationCode activationCode = await _activationCodeRepo.GetCodeByPhoneNumber(request.PhoneNumber, cancellationToken);

        if (activationCode is null) return SResult.Failure(Memos.ActivationCodeNotFound);

        else if (activationCode.Code != request.Code) return SResult.Failure(Memos.CodeIsIncorrect);

        else if (activationCode.ExpireDate < dtNow) return SResult.Failure(Memos.CodeHasExpired);

        else if (activationCode.Used) return SResult.Failure(Memos.CodeUsed);

        activationCode.Used = true;

        await _activationCodeRepo.UpdateAsync(activationCode, cancellationToken);

        User user = await _userRepo.GetUserByPhoneNumber(request.PhoneNumber, cancellationToken);

        if (user is null)
        {
            user = new()
            {
                RegisterDate = dtNow,
                LastLoginDate = dtNow,
                IsActive = true,
                PhoneNumber = request.PhoneNumber,
                Type = UserType.User
            };

            await _userRepo.AddAsync(user, cancellationToken);
        }
        else if (!user.IsActive) return SResult.Failure(Memos.UserIsNotActive);


        result.Tokens = await _jwtService.CreateUserJwtTokens(user, cancellationToken);
        result.UserInfo.UserName = user.UserName;
        result.UserInfo.FullName = user.FullName;

        return result;
    }
}
