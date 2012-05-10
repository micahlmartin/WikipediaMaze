using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardLeaderBadge : AwardBadgeBase
    {
        public AwardLeaderBadge(IRepository repository) : base(repository) { }

        protected override bool AllowMultiple
        {
            get { return true; }
        }

        protected override bool ShouldAwardBadge(User user)
        {
            return user.LeadingPuzzleCount >= 5;
        }

        protected override BadgeType BadgeType
        {
            get { return BadgeType.Leader; }
        }
    }
}
