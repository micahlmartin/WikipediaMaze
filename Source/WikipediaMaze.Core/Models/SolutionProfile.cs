using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikipediaMaze.Core
{
    public class SolutionProfile
    {
        public virtual int Id { get; private set; }
        public virtual int PuzzleId { get; private set; }
        public virtual int UserId { get; private set; }
        public virtual int PointsAwarded { get; private set; }
        public virtual int StepCount { get; private set; }
        public virtual DateTime DateCreated { get; private set; }
        public virtual string StartTopic { get; private set; }
        public virtual string EndTopic { get; private set; }
    }
}