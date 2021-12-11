using System.Linq;
using System.Linq.Dynamic.Core;

namespace common.Utilities.Paging
{
    public static class SortHelper
    {
        public static IQueryable Sort(this IQueryable collection, string sortBy, bool reverse = false)
        {
            return collection.OrderBy(sortBy + (reverse ? " descending" : ""));
        }
    }
}