using ServiceCenter.Domain.Entities.ReceiptAggregate;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

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

    public static IQueryable<Receipt> Sort(this IQueryable<Receipt> receipts, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return receipts;

        var orderParams = orderByQueryString.Trim().Split(',');
        var propertyInfos = typeof(Receipt).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();

        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param))
                continue;

            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

            if (objectProperty == null)
                continue;

            var direction = param.EndsWith(" desc") ? "descending" : "ascending";
            orderQueryBuilder.Append($"{objectProperty.Name} {direction}, ");
        }

        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
        if (string.IsNullOrWhiteSpace(orderQuery))
            return receipts;

        return receipts.OrderBy(orderQuery);
    }
}
