using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardDominatorBadge : AwardBadgeBase
    {
        public AwardDominatorBadge(IRepository repository) : base(repository) { }

        protected override bool AllowMultiple
        {
            get { return false; }
        }

        protected override bool ShouldAwardBadge(User user)
        {
            return user.LeadingPuzzleCount >= 25;
        }

        protected override BadgeType BadgeType
        {
            get { return BadgeType.Dominator; }
        }
    }
}
