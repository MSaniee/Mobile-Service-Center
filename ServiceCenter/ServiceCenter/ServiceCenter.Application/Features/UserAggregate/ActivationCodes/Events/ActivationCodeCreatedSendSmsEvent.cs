using MediatR;

namespace ServiceCenter.Application.Features.UserAggregate.ActivationCodes.Events;

public record ActivationCodeCreatedSendSmsEvent(
    int Code,
    string PhoneNumber)
    : INotification;

public class ActivationCodeCreatedSendSmsEventHandler : INotificationHandler<ActivationCodeCreatedSendSmsEvent>
{
    //private readonly ISmsService _smsService;

    //public ActivationCodeCreatedSendSmsEventHandler(ISmsService refreshTokenRepo)
    //{
    //    _smsService = refreshTokenRepo ?? throw new ArgumentNullException(nameof(refreshTokenRepo));
    //}

    public async Task Handle(ActivationCodeCreatedSendSmsEvent notification, CancellationToken cancellationToken)
    {
        string massage = "کدفعالسازی شما جهت ورود به بتا : \n"
                           + notification.Code
                           + "\n\n fJhzsRN1CZW";

        //if (!TestItem.IsProgrammer(notification.PhoneNumber)) await _smsService.SendAsync(notification.PhoneNumber, massage);
    }
}
