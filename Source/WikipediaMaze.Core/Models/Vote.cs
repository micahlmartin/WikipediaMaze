using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikipediaMaze.Core
{
    public class Vote
    {
        public virtual Guid Id { get; set; }
        public virtual int VoteId { get; set; }
        public virtual int UserId { get; set; }
        public virtual int PuzzleId { get; set; }
        public virtual VoteType VoteType { get; set; }
        public virtual DateTime DateVoted { get; set; }
    }
}
