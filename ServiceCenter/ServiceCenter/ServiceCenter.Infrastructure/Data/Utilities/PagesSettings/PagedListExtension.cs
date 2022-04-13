using ServiceCenter.Domain.Core.Utilities.PagesSettings;

namespace ServiceCenter.Infrastructure.Data.Utilities.PagesSettings;

public static class PagedListExtension
{
    public async static Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int page, int pageSize, CancellationToken cancellationToken)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PagedList<T>(items, count, page, pageSize);
    }

    public async static Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, Pagable pagable, CancellationToken cancellationToken)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source.Skip((pagable.Page - 1) * pagable.PageSize).Take(pagable.PageSize).ToListAsync(cancellationToken);

        return new PagedList<T>(items, count, pagable.Page, pagable.PageSize);
    }
}
