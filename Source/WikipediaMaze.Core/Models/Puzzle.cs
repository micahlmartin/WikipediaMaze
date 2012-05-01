using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace WikipediaMaze.Core
{
    public class Puzzle
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual string StartTopic { get; set; }
        public virtual string EndTopic { get; set; }
        public virtual User User { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual int Level { get; set; }
        public virtual IEnumerable<Solution> Solutions { get; set; }
        public virtual IEnumerable<Vote> Votes { get; set; }
        public virtual int VoteCount { get; set; }
        public virtual int SolutionCount { get; set; }
        public virtual bool IsVerified { get; set; }
        public virtual IEnumerable<string> Themes { get; set; }
        public virtual int LeaderId { get; set; }
        public virtual int CreatedById { get; set; }
        
        #endregion

        public static class Comparers
        {
            public readonly static IEqualityComparer<Puzzle> IdComparer = new PuzzleIdComparer();

            private class PuzzleIdComparer : IEqualityComparer<Puzzle>
            {

                public bool Equals(Puzzle x, Puzzle y)
                {
                    if (x == null && y == null)
                        return true;
                    else if (x == null)
                        return true;
                    else if (y == null)
                        return false;
                    else
                        return x.Id.Equals(y.Id);
                }

                public int GetHashCode(Puzzle obj)
                {
                    return obj.Id.GetHashCode();
                }
            }
        }
    }
}
