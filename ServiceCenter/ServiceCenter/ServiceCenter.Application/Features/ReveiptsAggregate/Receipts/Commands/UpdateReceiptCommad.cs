using MediatR;
using ServiceCenter.Application.Interfaces.Repositories.ReceiptAggregate;
using ServiceCenter.Common.Resources;
using ServiceCenter.Domain.Entities.ReceiptAggregate;
using ServiceCenter.Domain.Enums;

namespace ServiceCenter.Application.Features.ReveiptsAggregate.Receipts.Commands;

public record UpdateReceiptCommad(
        long Id,
        string Imei,
        MobileBrand MobileBrand,
        string MobileModel,
        string FaultDescription,
        string ImageUrl,
        Guid UserId)
        : IRequest<SResult>;

public class UpdateReceiptCommadHandler : IRequestHandler<UpdateReceiptCommad, SResult>
{
    private readonly IReceiptRepository _receiptRepo;

    public UpdateReceiptCommadHandler(
        IReceiptRepository receiptRepo)
    {
        _receiptRepo = receiptRepo.ThrowIfNull();
    }

    public async Task<SResult> Handle(UpdateReceiptCommad request, CancellationToken cancellationToken)
    {
        Receipt receipt = await _receiptRepo.GetByIdAsync(cancellationToken, request.Id);

        if (receipt is null) return SResult.Failure(Memos.ItemNotFound);

        receipt.FaultDescription = request.FaultDescription;
        receipt.ImageUrl = request.ImageUrl;
        receipt.Imei = request.Imei;
        receipt.MobileBrand = request.MobileBrand;
        receipt.MobileModel = request.MobileModel;

        await _receiptRepo.SaveChangeAsync(cancellationToken);

        return SResult.Success();
    }
}
