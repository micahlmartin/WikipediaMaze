using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardMysterioBadge : AwardBadgeBase
    {
        private int _badgeCount;

        public AwardMysterioBadge(IRepository repository) : base(repository) { }

        protected override bool AllowMultiple
        {
            get { return true; }
        }

        protected override bool ShouldAwardBadge(User user)
        {
            _badgeCount = Repository.All<Puzzle>().Count(x => x.CreatedById == user.Id && x.VoteCount >= 25);
            return _badgeCount > 0;
        }

        protected override void AssignBadgeCount(UserBadgeInfo badgeInfo)
        {
            badgeInfo.Count = _badgeCount;
        }

        protected override BadgeType BadgeType
        {
            get { return BadgeType.Mysterio; }
        }
    }
}
