using ServiceCenter.Domain.Entities.ReceiptAggregate;

namespace ServiceCenter.Infrastructure.Data.Repositories.ReceiptAggregate;

public static class RepositoryReceiptExtensions
{
    public static IQueryable<Receipt> Search(this IQueryable<Receipt> receipts, string searchTerm)
    {
        if (!searchTerm.HasValue()) return receipts;

        var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

        return receipts.Where(p => p.Imei.ToLower().Contains(lowerCaseSearchTerm) ||
                                   p.MobileModel.ToLower().Contains(lowerCaseSearchTerm));
    }
}
