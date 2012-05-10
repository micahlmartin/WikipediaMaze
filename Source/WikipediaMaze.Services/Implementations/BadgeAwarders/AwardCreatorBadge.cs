using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardCreatorBadge : AwardBadgeBase
    {
        public AwardCreatorBadge(IRepository repository) : base(repository) { }

        protected override bool AllowMultiple
        {
            get { return false; }
        }

        protected override bool ShouldAwardBadge(User user)
        {
            return Repository.All<Puzzle>().Any(x => x.CreatedById == user.Id);
        }

        protected override BadgeType BadgeType
        {
            get { return BadgeType.Creator; }
        }
    }
}
