using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using WikipediaMaze.Core;

namespace WikipediaMaze.Data.NHibernate.Mappings
{
    public class UserActionMap : ClassMap<UserAction>
    {
        public UserActionMap()
        {
            Table("Actions");
            Id(x => x.ActionId, "Id");
            Map(x => x.UserId);
            Map(x => x.Action)
                .Columns.Add("ActionType")
                .CustomType<UserActionType>();
                
            Map(x => x.DateCreated);
            Map(x => x.PuzzleId);
            Map(x => x.VoteType)
                .Columns.Add("VoteType")
                .CustomType<VoteType>();

            Map(x => x.SolutionId);
            Map(x => x.HasBeenChecked);
            Map(x => x.AffectedUserId);
        }
    }
}
