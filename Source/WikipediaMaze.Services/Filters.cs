using System.Linq;
using WikipediaMaze.Core;
using System.Collections.Generic;

namespace WikipediaMaze.Services
{
    public static class BadgeFilters
    {
        public static IEnumerable<Badge> ByBadgeLevel(this IEnumerable<Badge> badges, BadgeLevel level)
        {
            return badges.Where(b => b.Level == level);
        }
    }
}