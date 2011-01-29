using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikipediaMaze.Core
{
    public class Solution
    {
        #region Properties
        public virtual int Id { get; private set; }
        public virtual int UserId { get; set; }
        public virtual int PuzzleId { get; set; }
        public virtual IEnumerable<Step> Steps { get; set; }
        public virtual int PointsAwarded { get; set; }
        public virtual int StepCount { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual int CurrentPuzzleLevel { get; set; }
        public virtual int CurrentSolutionCount { get; set; }
        //public virtual Puzzle Puzzle { get; private set; }
        #endregion
        
    }
}
