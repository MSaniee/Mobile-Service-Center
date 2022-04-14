using MediatR;
using ServiceCenter.Application.Interfaces.Repositories.ReceiptAggregate;
using ServiceCenter.Common.Resources;
using ServiceCenter.Domain.Entities.ReceiptAggregate;

namespace ServiceCenter.Application.Features.ReveiptsAggregate.Receipts.Commands;

public record DeleteReceiptCommad(
    long Id)
    : IRequest<SResult>;

public class DeleteReceiptCommadHandler : IRequestHandler<DeleteReceiptCommad, SResult>
{
    private readonly IReceiptRepository _receiptRepo;

    public DeleteReceiptCommadHandler(
        IReceiptRepository receiptRepo)
    {
        _receiptRepo = receiptRepo.ThrowIfNull();
    }

    public async Task<SResult> Handle(DeleteReceiptCommad request, CancellationToken cancellationToken)
    {
        Receipt receipt = await _receiptRepo.GetByIdAsync(cancellationToken, request.Id);

        if (receipt is null) return SResult.Failure(Memos.ItemNotFound);

        await _receiptRepo.DeleteReceipt(receipt, cancellationToken);

        return SResult.Success();
    }
}

