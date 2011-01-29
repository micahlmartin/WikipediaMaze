using System.Collections.Generic;
using System.Linq;
using WikipediaMaze.Core;

namespace WikipediaMaze.Data
{
    public static class Filters
    {
        public static Theme ByName(this IQueryable<Theme> query, string name)
        {
            return query.Where(x => x.Name.Equals(name)).SingleOrDefault();
        }
        public static IQueryable<Puzzle> PlayedByUser(this IQueryable<Puzzle> query, int userId)
        {
            return query.Where(x => x.Solutions.Select(y => y.UserId).Contains(userId));
        }
        public static IQueryable<SolutionProfile> ByUserId(this IQueryable<SolutionProfile> query, int userId)
        {
            return query.Where(x => x.UserId == userId);
        }
        public static User ById(this IQueryable<User> query, int id)
        {
            return query.Where(u => u.Id == id).SingleOrDefault();
        }
        public static OpenIdentifier ByIdentifier(this IQueryable<OpenIdentifier> query, string identifier)
        {
            return query.Where(x => x.Identifier.Equals(identifier)).SingleOrDefault();
        }
        public static IQueryable<Solution> ByPuzzleId(this IQueryable<Solution> query, int id)
        {
            return query.Where(s => s.PuzzleId == id);
        }
        public static Puzzle ById(this IQueryable<Puzzle> query, int id)
        {
            return query.Where(p => p.Id == id).SingleOrDefault();
        }
        public static IEnumerable<Badge> ByBadgeLevel(this IEnumerable<Badge> badges, BadgeLevel level)
        {
            return badges.Where(b => b.Level == level);
        }
        public static Solution ById(this IQueryable<Solution> query, int solutionId)
        {
            return query.Where(x => x.Id == solutionId).SingleOrDefault();
        }
        public static IQueryable<Solution> ByUser(this IQueryable<Solution> query, int userId)
        {
            return query.Where(x => x.UserId == userId);
        }
        public static IQueryable<Puzzle> ByUser(this IQueryable<Puzzle> query, int userId)
        {
            return query.Where(x => x.User.Id == userId);
        }
    }
}