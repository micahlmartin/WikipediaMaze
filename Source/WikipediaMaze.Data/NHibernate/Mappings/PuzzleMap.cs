using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using WikipediaMaze.Core;

namespace WikipediaMaze.Data.NHibernate.Mappings
{
    public sealed class PuzzleMap : ClassMap<Puzzle>
    {
        public PuzzleMap()
        {
            Table("Puzzles");
            Id(x => x.Id);
            Map(x => x.StartTopic);
            Map(x => x.EndTopic);
            Map(x => x.DateCreated);
            Map(x => x.Level);
            Map(x => x.SolutionCount);
            Map(x => x.VoteCount);
            Map(x => x.IsVerified);
            Map(x => x.LeaderId);

            References(x => x.User)
                .Not.LazyLoad()
                .Column("CreatedById");

            HasMany(x => x.Solutions)
                .Table("Solutions")
                .KeyColumns.Add("PuzzleId");

            HasMany(x => x.Votes)
                .Table("Votes")
                .KeyColumns.Add("PuzzleId");

            HasManyToMany(x => x.Themes)
                .Not.LazyLoad()
                .Table("PuzzleThemes")
                .ParentKeyColumn("PuzzleId")
                .ChildKeyColumn("Theme");
        }
    }
}
