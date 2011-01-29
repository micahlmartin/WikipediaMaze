using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services
{
    public interface IGameService
    {
        CurrentPuzzleInfo StartPuzzle(int puzzleId);
        CurrentPuzzleInfo ContinuePuzzle(string topic);
        CurrentPuzzleInfo PuzzleInfo { get; }
        bool IsPuzzleInProgress { get; }
        CurrentPuzzleInfo GoBack();
    }
}
