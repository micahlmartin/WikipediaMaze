using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using WikipediaMaze.Core;
using WikipediaMaze.Data;
using System.IO;
using WikipediaMaze.Data.Mongo;

namespace WikipediaMaze.Services
{
    public class GameService : IGameService
    {
        #region Fields
        private readonly ITopicService _topicService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IPuzzleService _puzzleService;
        private readonly IPuzzleCache _puzzleCache;
        private readonly IAccountService _accountService;
        private readonly IRepository _repository;
        private readonly IReputationService _reputationService;
        private readonly IMessengerService _twitterService;

        #endregion

        #region Constructors

        public GameService(ITopicService topicService, IAuthenticationService authenticationService, IAccountService accountService, IPuzzleService puzzleService, IPuzzleCache puzzleCache, MongoRepository repository, IReputationService reputationService, IMessengerService twitterService)
        {
            _topicService = topicService;
            _authenticationService = authenticationService;
            _accountService = accountService;
            _puzzleService = puzzleService;
            _puzzleCache = puzzleCache;
            _repository = repository;
            _reputationService = reputationService;
            _twitterService = twitterService;
        }

        #endregion

        #region IGameService Members

        public CurrentPuzzleInfo GoBack()
        {
            #region Ensure

            if (!_authenticationService.IsAuthenticated)
                throw new UnauthorizedAccessException("You must be logged in to play Wikipedia Maze.");

            var puzzleInfo = _puzzleCache.CurrentPuzzleInfo;
            if (puzzleInfo == null)
                throw new InvalidOperationException("You must have a puzzle already in progress.");

            Puzzle puzzle = _puzzleService.GetPuzzleById(puzzleInfo.PuzzleId);
            if (puzzle == null)
                throw new FileNotFoundException("The puzzle you are trying to play '{0}' does not exist.".ToFormat(puzzleInfo.PuzzleId));

            #endregion

            Topic topicToGoBackTo;
            if (puzzleInfo.Steps.Count <= 1)
            {
                //Go back to the beginning
                puzzleInfo.CurrentTopic = null;
                topicToGoBackTo = puzzleInfo.StartTopic;
                puzzleInfo.Steps.Clear();
            }
            else
            {
                topicToGoBackTo = _topicService.GetTopicByName(puzzleInfo.Steps.Pop(puzzleInfo.Steps.Count - 1).Name);
                if (topicToGoBackTo == null)
                    throw new FileNotFoundException("The topic you requested does not exist.");

                //Verify that the selected step is actually linked from the current step and not
                //just typed in to the address bar.
                var previousTopic = puzzleInfo.Steps.ElementAt(puzzleInfo.Steps.Count - 1); //Go back only one because we popped the last one off of the stack already
                if (!previousTopic.RelatedTopics.Contains(topicToGoBackTo.Name, StringComparer.OrdinalIgnoreCase))
                    throw new InvalidOperationException("The topic you requested '{0}' is not directly related to the current topic '{1}'".ToFormat(topicToGoBackTo.Name.FormatTopic(), previousTopic.Name.FormatTopic()));

                puzzleInfo.CurrentTopic = topicToGoBackTo;
            }

            puzzleInfo.PreviousTopic = topicToGoBackTo;
            return puzzleInfo;
        }

        public CurrentPuzzleInfo StartPuzzle(int puzzleId)
        {
            _puzzleCache.ClearCurrentPuzzleInfo();

            #region Ensure

            var user = _accountService.GetUserById(_authenticationService.CurrentUserId);
            if (user == null)
                throw new UnauthorizedAccessException("You must be logged in to play a puzzle");

            Puzzle puzzle = _puzzleService.GetPuzzleById(puzzleId);
            if (puzzle == null)
                throw new FileNotFoundException("The puzzle you are trying to play '{0}' does not exist.".ToFormat(puzzleId));

            Topic startTopic = _topicService.GetTopicByName(puzzle.StartTopic);
            if (startTopic == null)
                throw new FileNotFoundException("The starting topic '{0}' could not be found".ToFormat(puzzle.StartTopic));

            if (!puzzle.IsVerified && puzzle.CreatedById != user.Id)
               throw new UnauthorizedAccessException("You are trying to access a puzzle that has not been verified and you aren't the creator.");

            #endregion

            var puzzleInfo = new CurrentPuzzleInfo
                                 {
                                     PuzzleId = puzzle.Id,
                                     UserId = user.Id,
                                     EndTopic = _topicService.GetTopicByName(puzzle.EndTopic),
                                     StartTopic = startTopic,
                                     CurrentTopic = null,
                                     PuzzleLevel = puzzle.Level
                                 };

            _puzzleCache.SetCurrentPuzzleInfo(puzzleInfo);

            return puzzleInfo;
        }

        public CurrentPuzzleInfo ContinuePuzzle(string topic)
        {
            #region Ensure
   
            if(!_authenticationService.IsAuthenticated)
                throw new UnauthorizedAccessException("You must be logged in to play Wikipedia Maze.");

            var puzzleInfo = _puzzleCache.CurrentPuzzleInfo;
            if (puzzleInfo == null)
                throw new InvalidOperationException("You must have a puzzle already in progress.");

            var newTopic = _topicService.GetTopicByName(topic);
            if (newTopic == null)
                throw new FileNotFoundException("The topic you requested '{0}' does not exist.".ToFormat(topic.FormatTopic()));

            if(puzzleInfo.CurrentTopic == null)
            {
                //This is the first step in the puzzle. 
                //Verify that the topic matches first step
                if(!puzzleInfo.StartTopic.Name.EqualsOrdinalIgnoreCase(topic))
                    throw new InvalidOperationException("The topic you requested '{0}' is not the first step in the puzzle.".ToFormat(topic.FormatTopic()));
            }
            else if(!puzzleInfo.IsGoingBack)
            {
                //Verify that the selected step is actually linked from the current step and not
                //just typed in to the address bar.
                var currentTopic = _topicService.GetTopicByName(puzzleInfo.CurrentTopic.Name);
                if(!currentTopic.RelatedTopics.Contains(topic, StringComparer.OrdinalIgnoreCase))
                    throw new InvalidOperationException("The topic you requested '{0}' is not directly related to the current topic '{1}'".ToFormat(topic.FormatTopic(), newTopic.Name.FormatTopic()));                    
            }
            #endregion

            if(puzzleInfo.CurrentTopic != null && 
                !puzzleInfo.CurrentTopic.Name.EqualsOrdinalIgnoreCase(topic) && !puzzleInfo.IsGoingBack)
            {
                //Add the previous step to the list if it's a different topic.
                    puzzleInfo.Steps.Add(puzzleInfo.CurrentTopic);
            }

            puzzleInfo.CurrentTopic = newTopic;

            if (IsFinalStep(puzzleInfo.EndTopic, newTopic))
            {
                CreateSolution(puzzleInfo);
                puzzleInfo.IsSolved = true;
            }

            puzzleInfo.PreviousTopic = null;
            return puzzleInfo;
        }

        private static bool IsFinalStep(Topic endTopic, Topic currentTopic)
        {
            if (RegularExpressions.DisambiguationTopicRegex.IsMatch(endTopic.Name))
            {
                //The end topic is very specific 
                //(i.e. contains disambiguation like: Canadian_Bacon_(film)
                //Therefore the final step must match exactly
                if (endTopic.Name.EqualsOrdinalIgnoreCase(currentTopic.Name))
                    return true;
            }

            if (RegularExpressions.DisambiguationTopicRegex.IsMatch(currentTopic.Name))
            {
                //The end topic is very specific 
                //(i.e. contains disambiguation like: Canadian_Bacon_(film)
                //but since the ending topic of the puzzle is not
                //then we'll just remove the disambiguation portion from the name
                var ambiguousTopicName = RegularExpressions.DisambiguationTopicRegex.Match(currentTopic.Name).Groups["Topic"].Value;
                if(endTopic.Name.EqualsOrdinalIgnoreCase(ambiguousTopicName))
                    return true;
            }

            //If we get here then try to compare the page titles in the event of a redirect. Otherwise just compare the topics
            return endTopic.PageTitle.EqualsOrdinalIgnoreCase(currentTopic.PageTitle) 
                || endTopic.Name.EqualsOrdinalIgnoreCase(currentTopic.Name);

            
        }
        private void CreateSolution(CurrentPuzzleInfo puzzleInfo)
        {
            var isValidating = false;
            var user = _repository.All<User>().ById(puzzleInfo.UserId);
            var puzzle = _repository.All<Puzzle>().ById(puzzleInfo.PuzzleId);
            var solution = new Solution
                                    {
                                        PuzzleId = puzzle.Id,
                                        UserId = puzzleInfo.UserId,
                                        PointsAwarded = _reputationService.CalculateSolutionReputation(puzzle.CreatedById, puzzleInfo.UserId, puzzleInfo.Steps.Count + 1, puzzle.Level),
                                        StepCount = puzzleInfo.Steps.Count + 1 /* add 1 for the final step */,
                                        DateCreated = DateTime.Now,
                                        CurrentPuzzleLevel = puzzle.Level,
                                        CurrentSolutionCount = puzzle.SolutionCount,
                                    };

            var steps = new List<Step>();

            for (var i = 0; i < puzzleInfo.Steps.Count; i++)
            {
                steps.Add(new Step
                               {
                                   StepNumber = i,
                                   Topic = puzzleInfo.Steps[i].Name,
                                   SolutionId = solution.SolutionId,
                               });
            }

            //final step
            steps.Add(new Step
                          {
                              StepNumber = puzzleInfo.Steps.Count,
                              Topic = puzzle.EndTopic,
                              SolutionId = solution.SolutionId
                          });

            solution.Steps = steps;
            solution.StepCount = steps.Count;

            _repository.Save(solution);

            puzzle.SolutionCount++;
            if (!puzzle.IsVerified)
            {
                isValidating = true;
                puzzle.IsVerified = true;
            }

            _repository.Save(puzzle);

            _repository.Save(new UserAction
                                 {
                                     Action = UserActionType.SolvedPuzzle,
                                     DateCreated = DateTime.Now,
                                     PuzzleId = puzzle.Id,
                                     SolutionId = solution.SolutionId,
                                     UserId = user.Id,
                                     AffectedUserId = puzzle.CreatedById
                                 });

            //Solution is commited at this point so if there is only 1 solution then we are the leader
            bool isLeader = _repository.All<Solution>().Where(x => x.PuzzleId == puzzle.Id && x.StepCount <= solution.StepCount).Count() == 1;

            _puzzleCache.ClearCurrentPuzzleInfo();

            _puzzleService.UpdatePuzzleStats(puzzle.Id);

            _reputationService.CalculateUserReputationForSolution(solution);

            if (isValidating)
                _twitterService.SendPuzzleCreatedMessage(puzzle.Id);

            if (isLeader)
                _twitterService.SendNewPuzzleLeaderMessage(user.Id, solution.StepCount, puzzle.Id);
        }

        public CurrentPuzzleInfo PuzzleInfo
        {
            get { return _puzzleCache.CurrentPuzzleInfo; }
        }

        public bool IsPuzzleInProgress
        {
            get { return _puzzleCache.CurrentPuzzleInfo != null; }
        }

        #endregion
    }
}
