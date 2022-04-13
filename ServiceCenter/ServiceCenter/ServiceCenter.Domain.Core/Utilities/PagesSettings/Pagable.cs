using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServiceCenter.Domain.Core.Utilities.PagesSettings
{
    public class Pagable
    {
        /// <summary>
        /// صفحه
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// تعداد در هر صفحه
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// جستجو
        /// </summary>
        [StringLength(100, MinimumLength = 2, ErrorMessage = "مقدار جستجو باید بین 2 تا 100 کارکاتر باشد")]
        public string Search { get; set; } = "";

        public string OrderBy { get; set; } = "name";
    }

    public static class PagableExtension
    {
        public static IQueryable<T> ToPaged<T>(this IQueryable<T> query, int page, int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static IQueryable<T> ToPaged<T>(this IQueryable<T> query, Pagable pagable)
        {
            return query.Skip((pagable.Page - 1) * pagable.PageSize).Take(pagable.PageSize);
        }

        public static IEnumerable<T> ToPagedAsEnumerable<T>(this IEnumerable<T> query, Pagable pagable)
        {
            return query.Skip((pagable.Page - 1) * pagable.PageSize).Take(pagable.PageSize);
        }

        public static List<T> ToPagedAsList<T>(this List<T> query, Pagable pagable)
        {
            return query.Skip((pagable.Page - 1) * pagable.PageSize).Take(pagable.PageSize).ToList();
        }
    }
}
