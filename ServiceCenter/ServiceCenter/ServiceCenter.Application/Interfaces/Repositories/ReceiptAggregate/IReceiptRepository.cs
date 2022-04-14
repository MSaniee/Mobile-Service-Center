using ServiceCenter.Application.Dtos.Receipts;
using ServiceCenter.Domain.Core.Utilities.PagesSettings;
using ServiceCenter.Domain.Entities.ReceiptAggregate;

namespace ServiceCenter.Application.Interfaces.Repositories.ReceiptAggregate;

public interface IReceiptRepository : IRepository<Receipt>
{
    Task<PagedList<ReceiptDto>> GetReceipts(Guid userId, Pagable pagable, CancellationToken cancellationToken);
    Task<ReceiptDto> GetById(long id, CancellationToken cancellationToken);
    Task DeleteReceipt(Receipt product, CancellationToken cancellationToken);
}

