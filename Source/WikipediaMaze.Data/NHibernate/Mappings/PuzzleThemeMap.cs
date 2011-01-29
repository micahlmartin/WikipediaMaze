using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using WikipediaMaze.Core;

namespace WikipediaMaze.Data.NHibernate.Mappings
{
    public class PuzzleThemeMap : ClassMap<PuzzleTheme>
    {
        public PuzzleThemeMap()
        {
            Table("PuzzleThemes");
            Id(x => x.Theme);

            References(x => x.Puzzle)
                .Not.LazyLoad()
                .Column("PuzzleId");
        }
    }
}