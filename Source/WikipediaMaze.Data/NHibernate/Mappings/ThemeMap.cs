using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using WikipediaMaze.Core;

namespace WikipediaMaze.Data.NHibernate.Mappings
{
    public class ThemeMap : ClassMap<Theme>
    {
        public ThemeMap()
        {
            Table("Themes");
            Id(x => x.Name);
            Map(x => x.DateCreated);
            Map(x => x.UserId);
            Map(x => x.Count);
        }
    }
}