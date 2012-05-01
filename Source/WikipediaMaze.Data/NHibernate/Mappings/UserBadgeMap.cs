using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using WikipediaMaze.Core;

namespace WikipediaMaze.Data.NHibernate
{
    public class UserBadgeMap : ClassMap<UserBadge>
    {
        public UserBadgeMap()
        {
            Table("UserBadges");
            Id(x => x.Id).Column("Id");
            Map(x => x.BadgeId);
            Map(x => x.UserId);

            References(x => x.Badge)
                .Not.LazyLoad()
                .Column("BadgeId");
        }
    }
}
