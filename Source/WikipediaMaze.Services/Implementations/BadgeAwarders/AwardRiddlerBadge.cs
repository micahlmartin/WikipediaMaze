using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Data;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardRiddlerBadge : AwardBadgeBase
    {
        private int _badgeCount;

        public AwardRiddlerBadge(IRepository repository) : base(repository) { }

        protected override bool AllowMultiple
        {
            get { return true; }
        }

        protected override bool ShouldAwardBadge(User user)
        {
            _badgeCount = Repository.All<Puzzle>().Count(x => x.VoteCount >= 5 && x.CreatedById == user.Id);
            return _badgeCount > 0;
        }

        protected override void AssignBadgeCount(UserBadgeInfo badgeInfo)
        {
            badgeInfo.Count = _badgeCount;
        }

        protected override BadgeType BadgeType
        {
            get { return BadgeType.Riddler; }
        }
    }
}
