using MediatR;
using ServiceCenter.Application.Dtos.ActivationCodes;
using ServiceCenter.Application.Features.UserAggregate.ActivationCodes.Events;
using ServiceCenter.Application.Interfaces.Repositories.UserAggregate;
using ServiceCenter.Common.Resources;
using ServiceCenter.Domain.Entities.UserAggregate;

namespace ServiceCenter.Application.Features.UserAggregate.ActivationCodes.Commands;

public record CreateActivationCodeCommand(
    string PhoneNumber)
    : IRequest<SResult<SendActivationCodeResultDto>>;

public class CreateActivationCodeCommandHandler : IRequestHandler<CreateActivationCodeCommand, SResult<SendActivationCodeResultDto>>
{
    private readonly IActivationCodeRepository _activationCodeRepo;
    private readonly IMediator _mediator;

    public CreateActivationCodeCommandHandler(
        IActivationCodeRepository activationCodeRepo,
        IMediator mediator)
    {
        _activationCodeRepo = activationCodeRepo.ThrowIfNull();
        _mediator = mediator.ThrowIfNull();
    }

    public async Task<SResult<SendActivationCodeResultDto>> Handle(CreateActivationCodeCommand request, CancellationToken cancellationToken)
    {
        SResult<SendActivationCodeResultDto> result = new();
        result.Data = new();

        DateTime dtNow = DateTime.Now;
        int code = true ? 1234 : new Random().Next(1000, 9999);

        ActivationCode activationCode = await _activationCodeRepo.GetCodeByPhoneNumber(request.PhoneNumber, cancellationToken);

        if (activationCode == null)
        {
            activationCode = new ActivationCode
            {
                PhoneNumber = request.PhoneNumber,
                Step = 0,
                Code = code,
                Used = false
            };
        }
        else if (activationCode.Used)
        {
            activationCode.Code = code;
            activationCode.Step = 0;
            activationCode.Used = false;
        }
        else if (activationCode.ExpireDate < dtNow)
        {
            activationCode.Code = code;
            activationCode.Step = ++activationCode.Step;
        }
        else
        {
            string remainingTime = (activationCode.ExpireDate - dtNow).ToString(@"hh\:mm\:ss");

            result.Data.CodeExpirationRemainingTime = remainingTime;
            result.IsSuccess = false;
            result.Message = Memos.ActivationCodeIsSended.Replace("{0}", remainingTime);

            return result;
        }

        activationCode.SetExpireDate(activationCode.Step);

        string expirationRemainingTime = (activationCode.ExpireDate - dtNow).ToString(@"hh\:mm\:ss");

        result.Data.CodeExpirationRemainingTime = expirationRemainingTime;
        result.IsSuccess = true;
        result.Message = Memos.ActivationCodeSentSuccessfully;

        await _activationCodeRepo.UpdateAsync(activationCode, cancellationToken);

        ActivationCodeCreatedSendSmsEvent notification = new(code, request.PhoneNumber);
        await _mediator.Publish(notification, cancellationToken);

        return result;
    }
}

