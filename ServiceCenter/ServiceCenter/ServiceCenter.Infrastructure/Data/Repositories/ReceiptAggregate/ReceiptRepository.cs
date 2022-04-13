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
                          .ProjectToType<ReceiptDto>()
                          .ToPagedListAsync(pagable, cancellationToken);



}

public static class RepositoryProductExtensions
{
    public static IQueryable<Receipt> Search(this IQueryable<Receipt> receipts, string searchTerm)
    {
        if (!searchTerm.HasValue()) return receipts;

        var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

        return receipts.Where(p => p.Imei.ToLower().Contains(lowerCaseSearchTerm) ||
                                   p.MobileModel.ToLower().Contains(lowerCaseSearchTerm));
    }
}
