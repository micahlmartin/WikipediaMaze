using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikipediaMaze.Core
{
    [Serializable]
    public class CurrentPuzzleInfo
    {
        private IList<Topic> _steps;
        public IList<Topic> Steps
        {
            get
            {
                if (_steps == null)
                    _steps = new List<Topic>();

                return _steps;
            }
        }

        public int PuzzleId { get; set; }
        public Topic StartTopic { get; set; }
        public Topic EndTopic { get; set; }
        public Topic CurrentTopic { get; set; }
        public int UserId { get; set; }
        public int PuzzleLevel { get; set; }
        public bool IsSolved { get; set; }
        public bool IsGoingBack
        {
            get { return PreviousTopic != null; }
        }
        public Topic PreviousTopic { get; set; }
    }
}
