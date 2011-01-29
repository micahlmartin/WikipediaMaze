using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikipediaMaze.Core
{
    public class CreatePuzzleResult
    {
        public CreatePuzzleResult(int puzzleId)
        {
            PuzzleId = puzzleId;
            Success = true;
        }
        public CreatePuzzleResult(IEnumerable<RuleViolation> ruleViolations)
        {
            RuleViolations = ruleViolations;
        }
        public CreatePuzzleResult(string message, IEnumerable<RuleViolation> ruleViolations)
        {
            Message = message;
            RuleViolations = ruleViolations;
        }
        public CreatePuzzleResult(RuleViolation ruleViolation)
        {
            _ruleViolations = new List<RuleViolation> { ruleViolation };
        }
        public CreatePuzzleResult(string message, RuleViolation ruleViolation)
        {
            Message = message;
            _ruleViolations = new List<RuleViolation> { ruleViolation };
        }

        private IEnumerable<RuleViolation> _ruleViolations;
        public IEnumerable<RuleViolation> RuleViolations
        {
            get
            {
                if (_ruleViolations == null)
                    _ruleViolations = new List<RuleViolation>();

                return _ruleViolations;
            }
            private set { _ruleViolations = value; }
        }

        public string Message { get; private set; }
        public bool Success { get; private set; }
        public int PuzzleId { get; private set; }
    }
}
