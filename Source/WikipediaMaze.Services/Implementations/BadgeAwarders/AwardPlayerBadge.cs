using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver.Builders;
using WikipediaMaze.Core;
using WikipediaMaze.Data;
using WikipediaMaze.Data.Mongo;
using MongoDB.Driver.Linq;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardPlayerBadge : AwardBadgeBase
    {
        public AwardPlayerBadge(IRepository repository) : base(repository) { }

        protected override bool AllowMultiple
        {
            get { return false; }
        }

        protected override bool ShouldAwardBadge(User user)
        {
            var puzzleIds = Repository.All<Solution>().Where(x => x.UserId == user.Id).Select(x => x.PuzzleId).Distinct().ToList();
            return Repository.All<Puzzle>().Any(x => x.Id.In(puzzleIds) && x.CreatedById != user.Id);
        }

        protected override BadgeType BadgeType
        {
            get { return BadgeType.Player; }
        }
    }
}
