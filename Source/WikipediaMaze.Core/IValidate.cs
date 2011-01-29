using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikipediaMaze.Core
{
    public interface IValidate
    {
        bool IsValid { get; }
        IEnumerable<RuleViolation> RuleViolations { get; }
    }
}
