using System;
using System.Collections.Generic;
using System.Linq;
using WikipediaMaze.Data;
using WikipediaMaze.Core;
using NHibernate;
using WikipediaMaze.Data.Mongo;

namespace WikipediaMaze.Services
{
    public class ReputationService : IReputationService
    {
        #region Fields

        private readonly IRepository _repository;

        #endregion

        #region Constructors

        public ReputationService(IRepository repository)
        {
            _repository = repository;
        }

        #endregion

        #region IReputationService Members

        private static int CalculateReputationFromVotes(IEnumerable<Vote> votes)
        {
            var reputation = 0;

            foreach (var vote in votes)
            {
                switch (vote.VoteType)
                {
                    case VoteType.Down:
                        reputation += Settings.DownVoteReputationValue;
                        break;
                    case VoteType.Up:
                        reputation += Settings.UpVoteReputationValue;
                        break;
                }
            }

            return reputation;
        }

        public int CalculateSolutionReputation(int puzzleUserId, int solutionUserId, int stepCount, int puzzleLevel)
        {
            //We don't award points for solving your own puzzle.
            //It's a requirement when you create the puzzle to solve it.
            if (puzzleUserId == solutionUserId)
                return 0;       

            /* 
             * Points are awarded based on the number of steps it took to solve
             * the puzzle in relation to the average.
             * If it took fewer steps than normal than they get more points
             * If it took longer then they get fewer points.
             * 
             * First calculate the difference between the number of steps and the average.
             * This number will be negative if it took longer.
             * Get the percentage of that number in relation to the average.
             * Multiply that percentage by the AverageSolutionReputationValue. 
             * This gives us the initial amount to award the user. 
             * If the user completed the puzzle in fewer steps than average than
             * we double that number to make sure we are awarding a healthy amount of points.
             * If the user completed them in more steps than average, this number is negative.
             * Add that result to the AverageSolutionReputationValue and that gives
             * you your final points to award.
            */


            var difference = puzzleLevel - stepCount;

            if (difference == 0)
                return Settings.AverageSolutionReputationValue;

            var percentage = Math.Round(difference / (double)puzzleLevel, 2);
                    
                //The amount of points awarded before the average is applied    
                double basePoints;
                if (puzzleLevel < stepCount) //Puzzle took longer than average
                    basePoints = (Settings.AverageSolutionReputationValue * percentage);
                else //Puzzle was completed in fewer steps than average.
                    basePoints = (Settings.AverageSolutionReputationValue * percentage * 2);

                //Add the number points awarded to the average
                var reputation = (int)Math.Round(basePoints + Settings.AverageSolutionReputationValue, 2);

                //Make sure that the user is always awarded something for their efforts.
                return Math.Max(reputation, Settings.MinimumSolutionReputationValue);
        }

        public void CalculateUserReputationForSolution(Solution currentSolution)
        {
            if (_repository.All<Puzzle>().ById(currentSolution.PuzzleId).CreatedById == currentSolution.UserId)
                return;

            var solutionUser = _repository.All<User>().ById(currentSolution.UserId);
            var puzzleUser = _repository.All<Puzzle>().ById(currentSolution.PuzzleId).User;
            var solutions = _repository.All<Solution>().ByPuzzleId(currentSolution.PuzzleId).ByUser(currentSolution.UserId);

            solutionUser.Reputation += CalculateSolutionUserReputation(currentSolution, solutions);

            if (solutions.Count() == 1)
                puzzleUser.Reputation += Settings.PointsAwardedToCreatorWhenPuzzleIsPlayed;

            _repository.Save(solutionUser);
            _repository.Save(puzzleUser);
        }

        private static int CalculateSolutionUserReputation(Solution currentSolution, IEnumerable<Solution> solutions)
        {
            var previousBestSolution = solutions
                .Where(x => x.SolutionId != currentSolution.SolutionId)
                .OrderByDescending(x => x.PointsAwarded)
                .FirstOrDefault();

            if (previousBestSolution == null)
                return currentSolution.PointsAwarded;

            var repToAward = 0;
            if (previousBestSolution.PointsAwarded < currentSolution.PointsAwarded)
                repToAward = currentSolution.PointsAwarded - previousBestSolution.PointsAwarded;

            return repToAward;
        }

        #endregion

    }
}
