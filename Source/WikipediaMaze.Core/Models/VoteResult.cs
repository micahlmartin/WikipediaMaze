using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikipediaMaze.Core
{
    /// <summary>
    /// Contains data indicating the status of a vote.
    /// </summary>
    public class VoteResult
    {
        public string ErrorMessage { get; set; }
        public VoteType? VoteType { get; set; }
        public int? VoteCount { get; set; }
    }
}
