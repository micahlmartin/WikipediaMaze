using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using WikipediaMaze.Core;

namespace WikipediaMaze.Data.NHibernate.Mappings
{
    public sealed class UserMap :  ClassMap<User>
    {
        public UserMap()
        {
            Table("Users");
            Id(x => x.Id);
            Map(x => x.LastVisit);
            Map(x => x.Reputation);
            Map(x => x.RealName);
            Map(x => x.DateCreated);
            Map(x => x.BirthDate);
            Map(x => x.Location);
            Map(x => x.Email);
            Map(x => x.Photo);
            Map(x => x.DisplayName);
            Map(x => x.PreferredUserName);
            Map(x => x.TwitterUserName);
            Map(x => x.LeadingPuzzleCount);
            HasMany(x => x.OpenIdentifiers)
                .Not.LazyLoad()
                .Table("OpenIdentifiers")
                .KeyColumns.Add("UserId");

            //HasMany(x => x.BadgeTypes).AsElement("BadgeId").CollectionType<BadgeType>()
            //    .Not.LazyLoad()
            //    .AsList()
            //    .WithTableName("UserBadges")
            //    .KeyColumnNames.Add("UserId");

            HasManyToMany(x => x.Badges)
                .Not.LazyLoad()
                .Table("UserBadges")
                .ParentKeyColumn("UserId")
                .ChildKeyColumn("BadgeId");
        }
    }
}
