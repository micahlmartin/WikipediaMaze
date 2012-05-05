using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using WikipediaMaze.Core;

namespace WikipediaMaze.Data.NHibernate.Mappings
{
    public sealed class VoteMap : ClassMap<Vote>
    {
        public VoteMap()
        {
            Table("Votes");
            Id(v => v.VoteId, "Id");
            Map(v => v.DateVoted);
            Map(v => v.PuzzleId);
            Map(v => v.UserId);
            Map(v => v.VoteType).CustomType<VoteType>();
        }
    }
}
