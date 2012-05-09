using System;
using WikipediaMaze.Core;
using WikipediaMaze.Data;
using WikipediaMaze.Services.Interfaces;
using System.Linq;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardBetaBadge : AwardBadgeBase
    {
        private static readonly DateTime CutoffDate = new DateTime(2010, 3, 1, 0, 0, 0, DateTimeKind.Utc);

        public AwardBetaBadge(IRepository repository) : base(repository) { }

        protected override bool ShouldAwardBadge(User user)
        {
            return user.DateCreated < CutoffDate;
        }

        protected override BadgeType BadgeType
        {
            get { return BadgeType.Beta; }
        }

        protected override bool AllowMultiple
        {
            get { return false; }
        }
    }
}
