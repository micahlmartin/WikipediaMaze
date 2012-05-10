﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver.Builders;
using WikipediaMaze.Core;
using WikipediaMaze.Data;
using WikipediaMaze.Data.Mongo;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardCriticBadge : AwardBadgeBase
    {
        public AwardCriticBadge(IRepository repository) : base(repository) { }

        protected override bool AllowMultiple
        {
            get { return false; }
        }

        protected override bool ShouldAwardBadge(Core.User user)
        {
            return MongoRepository.Database.GetCollection(MongoRepository.GetCollectionNamingConvention(typeof(Puzzle))).Count(Query.ElemMatch("Votes", Query.And(Query.EQ("UserId", user.Id), Query.EQ("VoteType", VoteType.Down)))) > 0;
        }

        protected override BadgeType BadgeType
        {
            get { return BadgeType.Critic; }
        }
    }
}
