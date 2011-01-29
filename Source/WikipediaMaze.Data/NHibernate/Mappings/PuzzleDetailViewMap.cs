using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;
using FluentNHibernate.Mapping;

namespace WikipediaMaze.Data.NHibernate.Mappings
{
    public sealed class PuzzleDetailViewMap : ClassMap<PuzzleDetailView>
    {
        public PuzzleDetailViewMap()
        {
            Table("PuzzleDetailView");
            Id(x => x.PuzzleId);
            Map(x => x.BronzeBadgeCount);
            Map(x => x.CreatedById);
            Map(x => x.DateCreated);
            Map(x => x.EndTopic);
            Map(x => x.GoldBadgeCount);
            Map(x => x.IsVerified);
            Map(x => x.LeaderId);
            Map(x => x.LeadingPuzzleCount);
            Map(x => x.Level);
            Map(x => x.Photo);
            Map(x => x.PuzzleId);
            Map(x => x.Reputation);
            Map(x => x.SolutionCount);
            Map(x => x.StartTopic);
            Map(x => x.Themes);
            Map(x => x.UserName);
            Map(x => x.VoteCount);
            Map(x => x.Email);
            Map(x => x.SilverBadgeCount);
        }
    }
}
