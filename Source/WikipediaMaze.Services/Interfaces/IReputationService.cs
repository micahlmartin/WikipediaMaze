using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services
{
    public interface IReputationService
    {
        int CalculateSolutionReputation(int puzzleUserId, int solutionUserId, int stepCount, int puzzleLevel);
        void CalculateUserReputationForSolution(Solution currentSolution);
    }
}
