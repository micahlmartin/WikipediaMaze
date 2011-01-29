using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using WikipediaMaze.Core;

namespace WikipediaMaze.Data.NHibernate.Mappings
{
    public sealed class SolutionMap : ClassMap<Solution>
    {
        public SolutionMap()
        {
            Table("Solutions");
            Id(s => s.Id);
            Map(x => x.PointsAwarded);
            Map(x => x.PuzzleId);
            Map(x => x.UserId);
            Map(x => x.StepCount);
            Map(x => x.DateCreated);
            Map(x => x.CurrentPuzzleLevel);
            Map(x => x.CurrentSolutionCount);

            //References(x => x.Puzzle)
            //    .Column("PuzzleId");

            HasMany(p => p.Steps)
                .Table("Step")
                .KeyColumns.Add("SolutionId");

        }
    }
}
