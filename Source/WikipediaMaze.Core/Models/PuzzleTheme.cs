using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikipediaMaze.Core
{
    public class PuzzleTheme
    {
        public virtual string Theme { get; set; }
        public virtual Puzzle Puzzle { get; set; }
    }
}