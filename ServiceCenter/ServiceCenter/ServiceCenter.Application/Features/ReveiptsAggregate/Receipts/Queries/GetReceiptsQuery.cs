using MediatR;
using ServiceCenter.Application.Dtos.Receipts;
using ServiceCenter.Application.Interfaces.Repositories.ReceiptAggregate;
using ServiceCenter.Domain.Core.Utilities.PagesSettings;

namespace ServiceCenter.Application.Features.ReveiptsAggregate.Receipts.Queries;

public record GetReceiptsQuery(
        Pagable Pagable,
        Guid UserId)
        : IRequest<SResult<PagedList<ReceiptDto>>>;

public class GetReceiptsQueryHandler : IRequestHandler<GetReceiptsQuery, SResult<PagedList<ReceiptDto>>>
{
    private readonly IReceiptRepository _receiptRepo;

    public GetReceiptsQueryHandler(
        IReceiptRepository receiptRepo)
    {
        _receiptRepo = receiptRepo.ThrowIfNull();
    }

    public async Task<SResult<PagedList<ReceiptDto>>> Handle(GetReceiptsQuery request, CancellationToken cancellationToken)
    {
        return await _receiptRepo.GetReceipts(request.UserId, request.Pagable, cancellationToken);
    }
}
