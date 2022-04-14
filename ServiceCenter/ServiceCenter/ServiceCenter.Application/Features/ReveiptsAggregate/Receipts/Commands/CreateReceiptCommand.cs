using Mapster;
using MediatR;
using ServiceCenter.Application.Interfaces.Repositories.ReceiptAggregate;
using ServiceCenter.Domain.Entities.ReceiptAggregate;
using ServiceCenter.Domain.Enums;

namespace ServiceCenter.Application.Features.ReveiptsAggregate.Receipts.Commands;

public record CreateReceiptCommand(
    string Imei,
    MobileBrand MobileBrand,
    string MobileModel,
    string FaultDescription,
    string ImageUrl,
    Guid UserId)
    : IRequest<SResult>;

public class CreateReceiptCommandHandler : IRequestHandler<CreateReceiptCommand, SResult>
{
    private readonly IReceiptRepository _receiptRepo;

    public CreateReceiptCommandHandler(
        IReceiptRepository receiptRepo)
    {
        _receiptRepo = receiptRepo.ThrowIfNull();
    }

    public async Task<SResult> Handle(CreateReceiptCommand request, CancellationToken cancellationToken)
    {
        Receipt receipt = request.Adapt<Receipt>();

        await _receiptRepo.AddAsync(receipt, cancellationToken);

        return SResult.Success();
    }
}


