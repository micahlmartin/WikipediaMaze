using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WikipediaMaze.Core;
using FluentNHibernate.Mapping;

namespace WikipediaMaze.Data.NHibernate.Mappings
{
    public class BadgeMap : ClassMap<Badge>
    {
        public BadgeMap()
        {
            Table("Badges");
            Id(x => x.Id).Column("Id");
            Map(x => x.Level)
                .Columns.Add("BadgeLevel")
                .CustomType<BadgeLevel>();
            Map(x => x.Name);
            Map(x => x.Description);
        }
    }
}
