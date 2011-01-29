using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;

namespace WikipediaMaze.Core
{
    public class PuzzleDetailView
    {
        public virtual int PuzzleId { get; set; }
        public virtual string StartTopic { get; set; }
        public virtual string EndTopic { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual int CreatedById { get; set; }
        public virtual int Level { get; set; }
        public virtual int VoteCount { get; set; }
        public virtual int SolutionCount { get; set; }
        public virtual bool IsVerified { get; set; }
        public virtual int LeaderId { get; set; }
        public virtual int Reputation { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Photo { get; set; }
        public virtual int LeadingPuzzleCount { get; set; }
        public virtual int BronzeBadgeCount { get; set; }
        public virtual int SilverBadgeCount { get; set; }
        public virtual int GoldBadgeCount { get; set; }
        public virtual string Themes { get; set; }
        public virtual string Email { get; set; }

        private IEnumerable<string> _themeList { get; set; }
        public virtual IEnumerable<string> ThemeList
        {
            get
            {
                if (_themeList == null && !string.IsNullOrWhiteSpace(Themes))
                    _themeList = Themes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                return _themeList;
            }
        }

        public virtual string GetCreatorGravatarUrl(int? size)
        {
            if (!string.IsNullOrEmpty(Email))
                return Email.AsGravatarUrl(size);

            return (UserName + "@wikipediamaze.com").AsGravatarUrl(size);
        }

        public static class Comparers
        {
            public readonly static IEqualityComparer<PuzzleDetailView> PuzzleIdComparer = new PuzzleDetailViewIdComparer();

            private class PuzzleDetailViewIdComparer : IEqualityComparer<PuzzleDetailView>
            {

                public bool Equals(PuzzleDetailView x, PuzzleDetailView y)
                {
                    if (x == null && y == null)
                        return true;
                    else if (x == null)
                        return true;
                    else if (y == null)
                        return false;
                    else
                        return x.PuzzleId.Equals(y.PuzzleId);
                }

                public int GetHashCode(PuzzleDetailView obj)
                {
                    return obj.PuzzleId.GetHashCode();
                }
            }
        }
    }
}
