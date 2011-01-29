using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WikipediaMaze.Core;
using FluentNHibernate.Mapping;

namespace WikipediaMaze.Data.NHibernate.Mappings
{
    public sealed class SolutionProfileMap : ClassMap<SolutionProfile>
    {
        public SolutionProfileMap()
        {
            Table("SolutionProfileView");
            Id(x => x.Id);
            Map(x => x.DateCreated);
            Map(x => x.EndTopic);
            Map(x => x.PointsAwarded);
            Map(x => x.PuzzleId);
            Map(x => x.StartTopic);
            Map(x => x.StepCount);
            Map(x => x.UserId);
        }
    }
}