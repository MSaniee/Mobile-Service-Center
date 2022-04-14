using MediatR;
using ServiceCenter.Application.Dtos.Receipts;
using ServiceCenter.Application.Interfaces.Repositories.ReceiptAggregate;

namespace ServiceCenter.Application.Features.ReveiptsAggregate.Receipts.Queries;

public record GetReceiptQuery(
    long Id)
    : IRequest<SResult<ReceiptDto>>;

public class GetReceiptQueryHandler : IRequestHandler<GetReceiptQuery, SResult<ReceiptDto>>
{
    private readonly IReceiptRepository _receiptRepo;

    public GetReceiptQueryHandler(
        IReceiptRepository receiptRepo)
    {
        _receiptRepo = receiptRepo.ThrowIfNull();
    }

    public async Task<SResult<ReceiptDto>> Handle(GetReceiptQuery request, CancellationToken cancellationToken)
    {
        return await _receiptRepo.GetById(request.Id, cancellationToken);
    }
}
