using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services.Interfaces
{
    public interface IAwardBadge
    {
        bool AwardBadge(UserAction action);
    }
}
