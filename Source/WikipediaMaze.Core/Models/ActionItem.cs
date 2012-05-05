using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikipediaMaze.Core
{
    public class UserAction
    {
        public virtual int ActionId { get; set; }
        public virtual Guid Id { get; set; }
        public virtual UserActionType Action { get; set; }
        public virtual int UserId { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual int? PuzzleId { get; set; }
        public virtual VoteType VoteType { get; set; }
        public virtual int? SolutionId { get; set; }
        public virtual bool HasBeenChecked { get; set; }
        public virtual int? AffectedUserId { get; set; }
    }
}
