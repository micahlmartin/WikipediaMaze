using System;
using System.Collections.Generic;
using System.Linq;

namespace WikipediaMaze.Core
{
    public class User : IValidate
    {
        public virtual int Id { get;  set; }
        public virtual DateTime LastVisit { get; set; }
        public virtual int Reputation { get; set; }
        public virtual IList<UserBadgeInfo> Badges { get; set; }    
        public virtual string RealName { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? BirthDate { get; set; }
        public virtual string Location { get; set; }
        public virtual string Email { get; set; }
        public virtual string Photo { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string PreferredUserName { get; set; }
        public virtual int LeadingPuzzleCount { get; set; }
        public virtual IEnumerable<OpenIdentifier> OpenIdentifiers { get; private set; }
        public virtual string TwitterUserName { get; set; }
        public virtual string GetGravatarUrl(int? size)
        {

            if (!string.IsNullOrEmpty(Email))
                return Email.AsGravatarUrl(size);

            return (PreferredUserName + "@wikipediamaze.com").AsGravatarUrl(size);
        }

        #region IValidate Members

        public virtual bool IsValid
        {
            get { return RuleViolations.Count() == 0; }
        }

        public virtual IEnumerable<RuleViolation> RuleViolations
        {
            get 
            {
                if (!string.IsNullOrEmpty(Email))
                {
                    if (!RegularExpressions.EmailRegex.IsMatch(Email))
                        yield return new RuleViolation("Email address is invalid.", "Email");
                }

                if (!string.IsNullOrEmpty(RealName))
                {
                    if (!RegularExpressions.UserNameRegex.IsMatch(RealName))
                        yield return new RuleViolation("Name is invalid.", "RealName");
                }

                if (string.IsNullOrEmpty(DisplayName))
                    yield return new RuleViolation("Display Name cannot be blank.", "DisplayName");
                else if (!RegularExpressions.UserNameRegex.IsMatch(DisplayName))
                    yield return new RuleViolation("Display Name is invalid.", "DisplayName");
            }
        }

        #endregion
    }
}