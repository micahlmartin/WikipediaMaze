﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikipediaMaze.Core
{
    public class ActionItem
    {
        public virtual int Id { get; private set; }
        public virtual ActionType Action { get; set; }
        public virtual int UserId { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual int? PuzzleId { get; set; }
        public virtual VoteType VoteType { get; set; }
        public virtual int? SolutionId { get; set; }
        public virtual bool HasBeenChecked { get; set; }
        public virtual int? AffectedUserId { get; set; }
    }
}
