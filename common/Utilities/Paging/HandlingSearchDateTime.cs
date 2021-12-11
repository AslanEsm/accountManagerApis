
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace common.Utilities.Paging
{
    public static class HandlingSearchDateTime
    {

        public static IQueryable<T> BetweenDates<T>(this IQueryable<T> inputQuery,
            string propertyName,
            string startDate = null,
            string endDate = null
            )
        {
            var query = inputQuery;

            if (startDate != null)
            {
                var start = startDate.ToMiladiDateTime();
                query = query.Where($"{propertyName} >= @0", start);
            }

            if (endDate != null)
            {
                var end = endDate.ToMiladiDateTime();
                query = query.Where($"{propertyName} <= @0", end);
            }

            if (startDate != null && endDate != null)
            {
                var start = startDate.ToMiladiDateTime();
                var end = endDate.ToMiladiDateTime();

                query = query.Where($"{propertyName} >= @0 && {propertyName} <= @1", start, end);
            }

            return query;
        }
    }
}
