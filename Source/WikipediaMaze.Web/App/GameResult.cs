using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WikipediaMaze.Core;

namespace WikipediaMaze.App
{
    public class GameResult
    {
        #region Constructors
        
        public GameResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
            Success = false;
        }
        public GameResult(Topic currentTopic)
        {
            CurrentTopic = currentTopic;
            Success = true;
        }
        public GameResult(bool isSolved, int puzzleId)
        {
            IsSolved = isSolved;
            Success = true;
            PuzzleId = puzzleId;
        }

        #endregion
        
        public string ErrorMessage { get; private set; }
        public bool Success { get; private set; }
        public Topic CurrentTopic { get; private set; }
        public bool IsSolved { get; private set; }
        public int PuzzleId { get; private set; }
    }
}
