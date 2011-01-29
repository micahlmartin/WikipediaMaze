using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services
{
    public interface IPuzzleCache
    {
        void ClearCurrentPuzzleInfo();
        CurrentPuzzleInfo CurrentPuzzleInfo { get; }
        void SetCurrentPuzzleInfo(CurrentPuzzleInfo puzzleInfo);
    }
}
