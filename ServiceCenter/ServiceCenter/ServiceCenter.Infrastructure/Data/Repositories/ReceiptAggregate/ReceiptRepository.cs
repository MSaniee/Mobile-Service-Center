using Mapster;
using ServiceCenter.Application.Dtos.Receipts;
using ServiceCenter.Application.Interfaces.Repositories.ReceiptAggregate;
using ServiceCenter.Domain.Core.DILifeTimesType;
using ServiceCenter.Domain.Core.Utilities.PagesSettings;
using ServiceCenter.Domain.Entities.ReceiptAggregate;
using ServiceCenter.Infrastructure.Data.SqlServer.EfCore.Context;
using ServiceCenter.Infrastructure.Data.Utilities.PagesSettings;

namespace ServiceCenter.Infrastructure.Data.Repositories.ReceiptAggregate;

public class ReceiptRepository : Repository<Receipt>, IReceiptRepository, IScopedDependency
{
    public ReceiptRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public Task<PagedList<ReceiptDto>> GetReceipts(Guid userId, Pagable pagable, CancellationToken cancellationToken)

        => TableNoTracking.Where(r => r.UserId == userId)
                          .Search(pagable.Search)
                          .Sort(pagable.OrderBy)
                          .ProjectToType<ReceiptDto>()
                          .ToPagedListAsync(pagable, cancellationToken);

    public Task<ReceiptDto> GetById(long id, CancellationToken cancellationToken)
    {
        return TableNoTracking.Where(r => r.Id == id).ProjectToType<ReceiptDto>().FirstOrDefaultAsync(cancellationToken);
    }

    public async Task DeleteReceipt(Receipt receipt, CancellationToken cancellationToken)
    {
        Delete(receipt);
        await SaveChangeAsync(cancellationToken);
    }
}
