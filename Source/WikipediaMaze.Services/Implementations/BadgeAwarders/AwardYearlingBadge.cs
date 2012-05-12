using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Data;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardYearlingBadge : AwardBadgeBase
    {
        public AwardYearlingBadge(IRepository repository) : base(repository) { }

        protected override bool AllowMultiple
        {
            get { return false; }
        }

        protected override bool ShouldAwardBadge(User user)
        {
            return DateTime.UtcNow.Date >= user.DateCreated.Date.AddYears(1);
        }

        protected override Core.BadgeType BadgeType
        {
            get { return BadgeType.Yearling; }
        }
    }
}
