using ServiceCenter.Application.Dtos.ActivationCodes;
using ServiceCenter.Application.Dtos.Users;
using ServiceCenter.Application.Features.UserAggregate.ActivationCodes.Commands;
using ServiceCenter.Application.Features.UserAggregate.Users;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceCenter.API.Areas.Users.Controllers.v1;

[Area("Users")]
[ApiVersion("1")]
[AllowAnonymous]
[ApiExplorerSettings(GroupName = "Users - Account")]
public class AccountController : BaseController
{
    private readonly IMediator _mediator;

    public AccountController(
        IMediator mediator)
    {
        _mediator = mediator.ThrowIfNull();
    }

    /// <summary>
    /// ارسال کدفعالسازی برای کاربر
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResult<SendActivationCodeResultDto>> SendActivationCode(SendActivationCodeDto dto, CancellationToken cancellationToken)
    {
        CreateActivationCodeCommand command = new(dto.PhoneNumber);

        return await _mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// ورود کاربر
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResult<UserLoginResultDto>> Login(UserLoginDto dto, CancellationToken cancellationToken)
    {
        RegisterOrLoginUserCommand command = new(dto.PhoneNumber, dto.Code);

        return await _mediator.Send(command, cancellationToken);
    }

}

