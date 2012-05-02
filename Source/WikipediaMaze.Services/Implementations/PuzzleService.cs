using System;
using System.Collections.Generic;
using System.Linq;
using WikipediaMaze.Data;
using MvcContrib.Pagination;
using WikipediaMaze.Core;
using System.Data.SqlClient;
using WikipediaMaze.Data.Mongo;

namespace WikipediaMaze.Services
{
	public sealed class PuzzleService : IPuzzleService
	{
		#region Fields

		private readonly IRepository _repository;
		private readonly IAccountService _accountService;
		private readonly IAuthenticationService _authenticationService;
		private readonly ITopicService _topicService;
		private readonly IReputationService _reputationService;

		#endregion  

		#region Constructors

		public PuzzleService(MongoRepository repository, IAccountService accountService, IAuthenticationService authenticationService, ITopicService topicService, IReputationService reputationService)
		{
			_repository = repository;
			_accountService = accountService;
			_authenticationService = authenticationService;
			_topicService = topicService;
			_reputationService = reputationService;
		}

		#endregion

		#region IPuzzleService Implementations
				
		/// <summary>
		/// Gets a puzzle based on the id.
		/// </summary>
		/// <param name="id">The id of the puzzle to retrieve.</param>
        public Puzzle GetPuzzleById(int id)
		{
		    var puzzle = _repository.All<Puzzle>().ById(id);
		    puzzle.User = _repository.All<User>().ById(puzzle.CreatedById);
		    return puzzle;
		}

        public void DeletePuzzle(int id)
        {
            if (!_authenticationService.IsAuthenticated)
                throw new UnauthorizedAccessException();

            var puzzle = _repository.All<Puzzle>().ById(id);

            if (puzzle.CreatedById != _authenticationService.CurrentUserId)
                throw new UnauthorizedAccessException();

            if (puzzle.IsVerified)
                throw new InvalidOperationException();

            _repository.Delete(puzzle);
        }

	    /// <summary>
		/// Creates a new puzzle
		/// </summary>
        public CreatePuzzleResult CreatePuzzle(string startTopic, string endTopic)
	    {
	        int id = 0;
	        User user = _authenticationService.CurrentUser;

	        #region Ensure

	        if (user == null)
	            return
	                new CreatePuzzleResult(new RuleViolation("You must be logged in to create a puzzle.", "Puzzle"));

	        var puzzleViolations = ValidatePuzzle(startTopic, endTopic);
	        if (puzzleViolations.Count() > 0)
	            return new CreatePuzzleResult(puzzleViolations);

	        if (DoesPuzzleExist(startTopic, endTopic))
	            return
	                new CreatePuzzleResult(
	                    new RuleViolation("A puzzle with the same start and end topic already exists.", "Puzzle"));

	        var topicStart = _topicService.GetTopicByUrl(startTopic);
	        if (topicStart != null &&
	            topicStart.RelatedTopics.Where(x => x.Equals(endTopic, StringComparison.OrdinalIgnoreCase)).
	                SingleOrDefault() != null)
	            return
	                new CreatePuzzleResult(
	                    new RuleViolation("The end topic must be more than 1 step away from the start topic.",
	                                      "EndTopic"));

	        var topicEnd = _topicService.GetTopicByUrl(endTopic);
	        if (topicStart.PageTitle.EqualsOrdinalIgnoreCase(topicEnd.PageTitle))
	            return new CreatePuzzleResult(
	                new RuleViolation("The end topic you selected is too similar to the start topic.", "EndTopic"));

	        #endregion

	        var puzzle = new Puzzle
	                         {
	                             StartTopic = _topicService.GetTopicNameFromUrl(startTopic),
	                             EndTopic = _topicService.GetTopicNameFromUrl(endTopic),
	                             User = user,
	                             DateCreated = DateTime.Now,
	                             Level = 0,
	                             SolutionCount = 0,
	                             VoteCount = 0,
	                             IsVerified = false,
	                         };

	        _repository.Save(puzzle);
	        _repository.Save(new ActionItem
	                             {
	                                 Action = ActionType.CreatedPuzzle,
	                                 DateCreated = DateTime.Now,
	                                 PuzzleId = puzzle.Id,
	                                 UserId = user.Id
	                             });
	        id = puzzle.Id;
	        return new CreatePuzzleResult(id);
	    }

	    private IEnumerable<RuleViolation> ValidatePuzzle(string startTopic, string endTopic)
		{
			var errorMessage = ValidateTopicUrl(startTopic); 
			if (errorMessage != string.Empty)
				yield return new RuleViolation(errorMessage, "StartTopic");
			else if(!_topicService.DoesTopicExist(startTopic))
				yield return new RuleViolation("The topic could not be found", "StartTopic");

			errorMessage = ValidateTopicUrl(endTopic);
			if (errorMessage != string.Empty)
				yield return new RuleViolation(errorMessage, "EndTopic");
			else if(endTopic.Equals(startTopic, StringComparison.InvariantCultureIgnoreCase))
				yield return new RuleViolation("The start and end topic cannot be the same", "EndTopic");
			else if(!_topicService.DoesTopicExist(endTopic))
				yield return new RuleViolation("The topic could not be found", "EndTopic");

			if(_topicService.TopicContainsRelatedTopic(startTopic, endTopic))
				yield return new RuleViolation("The end topic must be more than one step away from the starting topic.", "Puzzle");
				
		}
		private static string ValidateTopicUrl(string topicUrl)
		{
			if (string.IsNullOrEmpty(topicUrl))
				return "Topic cannot be blank.";

			if (!Uri.IsWellFormedUriString(topicUrl, UriKind.Absolute))
				return "Topic is not a valid url.";

			if (!IsValidWikipediaLink(topicUrl))
				return "Topic is not a valid wikipedia link.";

			if (!IsValidWikipediaTopic(topicUrl))
				return "Topic is not valid.";

			return string.Empty;
		}
		private static bool IsValidWikipediaLink(string topicUrl)
		{
			var match = RegularExpressions.WikipediaLinkRegex.Match(topicUrl);
			var group = match.Groups["Url"];
			return group.Success;
		}
		private static bool IsValidWikipediaTopic(string topicUrl)
		{
			var match = RegularExpressions.WikipediaLinkRegex.Match(topicUrl);
			var group = match.Groups["Topic"];

			return group.Success;
			//if (!group.Success) return false;

			//var topic = group.Value;
			//return !GlobalData.IllegalTopicCharactersRegex.IsMatch(topic);
		}
		private bool DoesPuzzleExist(string startTopicUrl, string endTopicUrl)
		{
			var startTopicName = _topicService.GetTopicNameFromUrl(startTopicUrl);
			var endTopicName = _topicService.GetTopicNameFromUrl(endTopicUrl);

				return
					_repository.All<Puzzle>().Where(
						p =>
						p.StartTopic.Equals(startTopicName, StringComparison.InvariantCultureIgnoreCase) &&
						p.EndTopic.Equals(endTopicName, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault() !=
					null;
		}

		/// <summary>
		/// Updates the vote count of the puzzle
		/// </summary>
		/// <param name="puzzleId">The id of the puzzle to update.</param>
		/// <returns>The updated vote count.</returns>
        public int UpdatePuzzleVoteCount(int puzzleId)
		{
		    int puzzleVoteCount = _repository.All<Vote>().Where(p => p.PuzzleId == puzzleId).Sum(p => (int) p.VoteType);
		    var puzzle = _repository.All<Puzzle>().ById(puzzleId);

		    puzzle.VoteCount = puzzleVoteCount;

		    _repository.Save(puzzle);

		    return puzzleVoteCount;
		}

		public void UpdatePuzzleStats(int puzzleId)
		{
			using (var connection = new SqlConnection(Settings.WikipediaMazeConnection))
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = "EXEC UpdatePuzzleStats {0}".ToFormat(puzzleId);
					command.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Returns all the votes for a specific user for each puzzle in the collection
		/// </summary>
		/// <param name="puzzles">The collection of puzzles to retrieve the votes for</param>
		/// <param name="userId">The user id of votes to retrieve</param>
		/// <returns>A collection votes</returns>
        public IEnumerable<Vote> GetVotes(IEnumerable<Puzzle> puzzles, int userId)
		{
		    return puzzles.Select(puzzle => _repository.All<Vote>().Where(x => x.PuzzleId == puzzle.Id && x.UserId == userId).OrderByDescending(x => x.DateVoted).FirstOrDefault()).Where(vote => vote != null).ToList();
		}

        public IEnumerable<Vote> GetVotes(IEnumerable<int> puzzleIds, int userId)
        {
            var ids = puzzleIds.ToList();
            return _repository.All<Vote>().Where(x => ids.Contains(x.PuzzleId) && x.UserId == userId).ToList();
        }

        public IEnumerable<Vote> GetVotes(IEnumerable<PuzzleDetailView> puzzles, int userId)
        {
            return puzzles.Select(puzzle => _repository.All<Vote>().Where(x => x.PuzzleId == puzzle.PuzzleId && x.UserId == userId).OrderByDescending(x => x.DateVoted).FirstOrDefault()).Where(vote => vote != null).ToList();
        }

	    /// <summary>
		/// Returns a list of Puzzle Id's where the specified user is in the lead.
		/// </summary>
		/// <param name="puzzles">A collection of puzzles that is being checked to see if the user is in the lead</param>
		/// <param name="userId">The id of the user</param>
		/// <returns></returns>
        public IEnumerable<int> GetPuzzlesLedByUser(IEnumerable<Puzzle> puzzles, int userId)
	    {
	        return GetPuzzlesLedByUserInternal(puzzles, userId);
	    }

	    /// <summary>
		/// Get's a list of all puzzles that the user is leading.
		/// </summary>
        public IEnumerable<int> GetPuzzlesLedByUser(int userId)
	    {
	        return GetPuzzlesLedByUserInternal(_repository.All<Puzzle>().ByUser(userId), userId);
	    }

	    private IEnumerable<int> GetPuzzlesLedByUserInternal(IEnumerable<Puzzle> puzzles, int userId)
		{
			var puzzleIds = new List<int>();

			foreach (var puzzle in puzzles)
			{
				var solutions = _repository.All<Solution>()
								.ByPuzzleId(puzzle.Id);

				//No solutions for the puzzle
				if (solutions.Count() == 0) continue;


				var stepCount = solutions.Min(x => x.StepCount);

				var topSolution = solutions.Where(x => x.StepCount == stepCount)
											.OrderBy(x => x.DateCreated).FirstOrDefault();

				bool isLeader = false;
				if (topSolution.UserId == userId)
					isLeader = true;

				if (isLeader)
					puzzleIds.Add(puzzle.Id);
			}

			return puzzleIds;
		}

        public IPagination<Puzzle> GetPuzzlesByUserId(int userId, PuzzleSortType sort, int page, int pageSize)
        {
            pageSize = Math.Min(pageSize, Settings.DefualtPageSize);

            IQueryable<Puzzle> puzzles;
            switch (sort)
            {
                case PuzzleSortType.Newest:
                    puzzles = _repository.All<Puzzle>().Where(x => x.User.Id == userId).OrderByDescending(p => p.DateCreated).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                case PuzzleSortType.Solutions:
                    puzzles = _repository.All<Puzzle>().Where(x => x.User.Id == userId).OrderByDescending(p => p.SolutionCount).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                case PuzzleSortType.Level:
                    puzzles = _repository.All<Puzzle>().Where(x => x.User.Id == userId).OrderByDescending(p => p.Level).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                case PuzzleSortType.Votes:
                    puzzles = _repository.All<Puzzle>().Where(x => x.User.Id == userId).OrderByDescending(p => p.VoteCount).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("sort");
            }
            return new CustomPagination<Puzzle>(puzzles.ToList(), page, pageSize, _repository.All<Puzzle>().Where(x => x.User.Id == userId).Count());
        }

		/// <summary>
		/// Votes on a puzzle.
		/// </summary>
		/// <remarks>If the puzzle has already been in the specified direction then the vote is removed.</remarks>
		/// <param name="puzzleId">The puzzle to vote up</param>
		/// <param name="voteType">The type of vote to apply to the puzzle</param>
        public VoteResult VoteOnPuzzle(int puzzleId, VoteType voteType)
		{
		    Puzzle puzzle = null;
		    VoteResult voteResult = null;
		    var puzzleCreatorRep = 0;
		    var voterRep = 0;

		    var user = _repository.All<User>().ById(_authenticationService.CurrentUserId);

		    #region Ensure

		    //Make sure the puzzle exists
		    puzzle = _repository.All<Puzzle>().ById(puzzleId);
		    if (puzzle == null)
		        return new VoteResult {ErrorMessage = "The puzzle you requested does not exist."};

		    //Only logged in users can vote
		    if (!_authenticationService.IsAuthenticated)
		        return new VoteResult {ErrorMessage = "You must be logged in to vote on a puzzle"};

		    //Make sure the user has enough reputation to vote
		    if (voteType == VoteType.Up && user.Reputation < Settings.MinimumReputationToUpVote)
		        return new VoteResult
		                   {
		                       ErrorMessage =
		                           "You must have a reputation of at least {0} to up vote a puzzle".ToFormat(
		                               Settings.MinimumReputationToUpVote)
		                   };
		    if (voteType == VoteType.Down && user.Reputation < Settings.MinimumReputationToDownVote)
		        return new VoteResult
		                   {
		                       ErrorMessage =
		                           "You must have a reputation of at least {0} to down vote a puzzle".ToFormat(
		                               Settings.MinimumReputationToDownVote)
		                   };

		    //Make sure the user is not voting on their own puzzle.
            if (user.Id == puzzle.CreatedById)
		        return new VoteResult {ErrorMessage = "You cannot vote on your own puzzle."};

		    //Make sure the user hasn't reached their vote limit for the day
		    var todaysNumberOfVotes =
		        _repository.All<Vote>().Where(x => x.UserId == user.Id && x.DateVoted == DateTime.Now.Date).
		            Count();

		    if (todaysNumberOfVotes >= Settings.MaximumDailyVoteLimit)
		        return new VoteResult
		                   {
		                       ErrorMessage =
		                           "You have reached the daily vote limit of {0}. Please wait a little while before voting again."
		                           .ToFormat(Settings.MaximumDailyVoteLimit)
		                   };

		    #endregion

		    //Begin transaction
		    voteResult = new VoteResult();
		    puzzleCreatorRep += GetPointsForVotee(voteType);
		    voterRep += GetPointsForVoter(voteType);

		    //Check for an existing vote record.
		    var vote = _repository.All<Vote>().Where(v => v.UserId == user.Id && v.PuzzleId == puzzleId).FirstOrDefault();
		    if (vote == null)
		    {
		        //No vote exists so create a new vote.
		        vote = new Vote
		                   {
		                       DateVoted = DateTime.Now.Date,
		                       PuzzleId = puzzleId,
		                       UserId = user.Id,
		                       VoteType = voteType
		                   };

		        _repository.Save(vote);

		        voteResult.VoteType = voteType;
		    }
		    else
		    {
		        //Determine which vote type to apply
		        var voteTypeToApply = VoteType.None;

		        if (voteType == VoteType.Down && vote.VoteType == VoteType.Down)
		        {
		            puzzleCreatorRep *= -1;
		            voterRep *= -1;
		            voteTypeToApply = VoteType.None;
		        }
		        else if (voteType == VoteType.Up && vote.VoteType == VoteType.Up)
		        {
		            puzzleCreatorRep *= -1;
		            voterRep *= -1;
		            voteTypeToApply = VoteType.None;
		        }
		        else
		            voteTypeToApply = voteType;


		        if (voteTypeToApply == VoteType.None)
		            _repository.Delete(vote);
		        else
		        {
		            vote.VoteType = voteTypeToApply;
		            _repository.Save(vote);
		        }

		        voteResult.VoteType = voteTypeToApply;
		    }

		    puzzle.User.Reputation += puzzleCreatorRep;
		    user.Reputation += voterRep;

		    _repository.Save(puzzle.User);
		    _repository.Save(user);

		    _repository.Save(new ActionItem
		                         {
		                             Action = ActionType.Voted,
		                             DateCreated = DateTime.Now,
		                             UserId = user.Id,
		                             PuzzleId = puzzleId,
		                             VoteType = voteResult.VoteType.Value,
                                     AffectedUserId = puzzle.CreatedById
		                         });

		    //Update Puzzle Vote Count
		    voteResult.VoteCount = UpdatePuzzleVoteCount(puzzleId);

		    return voteResult;
		}

	    private static int GetPointsForVotee(VoteType vote)
		{
			switch (vote)
			{
				case VoteType.None:
					return 0;
				case VoteType.Up:
					return Settings.UpVoteReputationValue;
				case VoteType.Down:
					return Settings.DownVoteReputationValue;
			}
			return 0;
		}
		private static int GetPointsForVoter(VoteType vote)
		{
			return 0;
		}

        public IEnumerable<Theme> GetAllThemes()
        {
            return _repository.All<Theme>().ToList();
        }

	    /// <summary>
		/// Gets a collection of themes
		/// </summary>
		/// <param name="page">The page number to return.</param>
		/// <param name="pageSize">The size of each page.</param>
		public IPagination<TopicTheme> GetTopThemes(int? page, int? pageSize)
		{
			page = page ?? 1;
			pageSize = pageSize ?? Settings.DefualtPageSize;

			var themeDictionary = new Dictionary<string, int>();

			var fromPuzzles = _repository.All<Puzzle>().Where(x => x.IsVerified).Select(p => p.StartTopic).ToList();
			var groupedFromPuzzles = fromPuzzles.GroupBy(p => p).OrderByDescending(p => p.Count()); //Put on separate line due to NHibernate.Linq bug.
			PopulateThemeDictionary(themeDictionary, groupedFromPuzzles);

			var toPuzzles = _repository.All<Puzzle>().Where(x => x.IsVerified).Select(p => p.EndTopic).ToList();
			var groupedToPuzzles = toPuzzles.GroupBy(p => p).OrderByDescending(p => p.Count());
			PopulateThemeDictionary(themeDictionary, groupedToPuzzles);
			

			var themes = new List<TopicTheme>();
			foreach (var i in themeDictionary)
			{
				themes.Add(new TopicTheme
							   {
								   Name = i.Key,
								   Count = i.Value
							   });
			}

			return themes.AsPagination(page.Value, pageSize.Value);
		}
		private static void PopulateThemeDictionary(IDictionary<string, int> themes, IEnumerable<IGrouping<string, string>> themeGroupings)
		{
			foreach (var puzzle in themeGroupings)
			{
				if (!themes.ContainsKey(puzzle.Key))
					themes.Add(puzzle.Key, puzzle.Count());
				else
					themes[puzzle.Key] += puzzle.Count();
			}
		}

		/// <summary>
		/// Returns a collection of puzzles
		/// </summary>
		/// <param name="sort">The type of sort to apply to the collection.</param>
		/// <param name="page">The page number of the collection to return. The default is 1.</param>
		/// <param name="pageSize">The number of puzzles to return in each page.</param>
        public IPagination<Puzzle> GetPuzzles(PuzzleSortType sort, int page, int pageSize)
        {
            IEnumerable<Puzzle> puzzles;

            switch (sort)
            {
                case PuzzleSortType.Newest:
                    puzzles = _repository.All<Puzzle>().Where(x => x.IsVerified).OrderByDescending(p => p.DateCreated).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case PuzzleSortType.Solutions:
                    puzzles = _repository.All<Puzzle>().Where(x => x.IsVerified).OrderByDescending(p => p.SolutionCount).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case PuzzleSortType.Level:
                    puzzles = _repository.All<Puzzle>().Where(x => x.IsVerified).OrderByDescending(p => p.Level).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case PuzzleSortType.Votes:
                    puzzles = _repository.All<Puzzle>().Where(x => x.IsVerified).OrderByDescending(p => p.VoteCount).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("sort");
            }
            return new CustomPagination<Puzzle>(puzzles, page, pageSize, _repository.All<Puzzle>().Where(x => x.IsVerified).Count());
        }

        public IPagination<PuzzleDetailView> GetPuzzleDetailView(PuzzleSortType sort, int page, int pageSize)
        {
            IEnumerable<PuzzleDetailView> puzzles;

            switch (sort)
            {
                case PuzzleSortType.Newest:
                    puzzles = _repository.All<PuzzleDetailView>().Where(x => x.IsVerified).OrderByDescending(p => p.DateCreated).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case PuzzleSortType.Solutions:
                    puzzles = _repository.All<PuzzleDetailView>().Where(x => x.IsVerified).OrderByDescending(p => p.SolutionCount).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case PuzzleSortType.Level:
                    puzzles = _repository.All<PuzzleDetailView>().Where(x => x.IsVerified).OrderByDescending(p => p.Level).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case PuzzleSortType.Votes:
                    puzzles = _repository.All<PuzzleDetailView>().Where(x => x.IsVerified).OrderByDescending(p => p.VoteCount).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("sort");
            }
            return new CustomPagination<PuzzleDetailView>(puzzles, page, pageSize, _repository.All<PuzzleDetailView>().Where(x => x.IsVerified).Count());
        }

        public IPagination<Puzzle> GetPuzzles(PuzzleSortType sort, int page, int pageSize, IEnumerable<string> themes)
        {
            IList<Puzzle> puzzles = new List<Puzzle>();

            foreach (var theme in themes)
            {
                var currentTheme = theme;
                var themedPuzzles = _repository.All<PuzzleTheme>().Where(x => x.Theme.Equals(currentTheme, StringComparison.OrdinalIgnoreCase) && x.Puzzle.IsVerified).Select(x => x.Puzzle).ToList();
                if (themedPuzzles.Count > 0)
                    puzzles.AddRange(themedPuzzles);
            }


            puzzles = puzzles.Distinct(Puzzle.Comparers.IdComparer).ToList();

            switch (sort)
            {
                case PuzzleSortType.Newest:
                    puzzles = puzzles.Where(x => x.IsVerified).OrderByDescending(p => p.DateCreated).ToList();
                    break;
                case PuzzleSortType.Solutions:
                    puzzles = puzzles.Where(x => x.IsVerified).OrderByDescending(p => p.SolutionCount).ToList();
                    break;
                case PuzzleSortType.Level:
                    puzzles = puzzles.Where(x => x.IsVerified).OrderByDescending(p => p.Level).ToList();
                    break;
                case PuzzleSortType.Votes:
                    puzzles = puzzles.Where(x => x.IsVerified).OrderByDescending(p => p.VoteCount).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("sort");
            }
            return new CustomPagination<Puzzle>(puzzles.Skip((page - 1) * pageSize).Take(pageSize), page, pageSize, puzzles.Count());
        }
        public IPagination<PuzzleDetailView> GetPuzzleDetailView(PuzzleSortType sort, int page, int pageSize, IEnumerable<string> themes)
        {
            IList<PuzzleDetailView> puzzles = new List<PuzzleDetailView>();

            foreach (var theme in themes)
            {
                var currentTheme = theme;
                puzzles.AddRange(_repository.All<PuzzleDetailView>().Where(x => x.Themes.Contains(theme)).ToList());
            }


            puzzles = puzzles.Distinct(PuzzleDetailView.Comparers.PuzzleIdComparer).ToList();

            switch (sort)
            {
                case PuzzleSortType.Newest:
                    puzzles = puzzles.Where(x => x.IsVerified).OrderByDescending(p => p.DateCreated).ToList();
                    break;
                case PuzzleSortType.Solutions:
                    puzzles = puzzles.Where(x => x.IsVerified).OrderByDescending(p => p.SolutionCount).ToList();
                    break;
                case PuzzleSortType.Level:
                    puzzles = puzzles.Where(x => x.IsVerified).OrderByDescending(p => p.Level).ToList();
                    break;
                case PuzzleSortType.Votes:
                    puzzles = puzzles.Where(x => x.IsVerified).OrderByDescending(p => p.VoteCount).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("sort");
            }
            return new CustomPagination<PuzzleDetailView>(puzzles.Skip((page - 1) * pageSize).Take(pageSize), page, pageSize, puzzles.Count());
        }
		public IPagination<Puzzle> GetPuzzlesByTheme(string theme, PuzzleSortType sort, int? page, int? pageSize)
		{
			page = page ?? 1;
			pageSize = pageSize ?? Settings.DefualtPageSize;

			switch (sort)
			{
				case PuzzleSortType.Newest:
					return _repository.All<Puzzle>().Where(x => x.IsVerified).Where(x => x.StartTopic.Equals(theme, StringComparison.InvariantCulture) || x.EndTopic.Equals(theme, StringComparison.InvariantCulture)).OrderByDescending(p => p.DateCreated).AsPagination(page.Value, pageSize.Value);
				case PuzzleSortType.Solutions:
					return _repository.All<Puzzle>().Where(x => x.IsVerified).Where(x => x.StartTopic.Equals(theme, StringComparison.InvariantCulture) || x.EndTopic.Equals(theme, StringComparison.InvariantCulture)).OrderByDescending(p => p.SolutionCount).AsPagination(page.Value, pageSize.Value);
				case PuzzleSortType.Level:
					return _repository.All<Puzzle>().Where(x => x.IsVerified).Where(x => x.StartTopic.Equals(theme, StringComparison.InvariantCulture) || x.EndTopic.Equals(theme, StringComparison.InvariantCulture)).OrderByDescending(p => p.Level).AsPagination(page.Value, pageSize.Value);
				case PuzzleSortType.Votes:
					return _repository.All<Puzzle>().Where(x => x.IsVerified).Where(x => x.StartTopic.Equals(theme, StringComparison.InvariantCulture) || x.EndTopic.Equals(theme, StringComparison.InvariantCulture)).OrderByDescending(p => p.VoteCount).AsPagination(page.Value, pageSize.Value);
				default:
					throw new ArgumentOutOfRangeException("sort");
			}
		}
        public IEnumerable<Step> GetSteps(int solutionId)
        {
            return _repository.All<Step>().Where(x => x.SolutionId == solutionId).ToList();
        }

        public IEnumerable<Solution> GetSolutions(int puzzleId)
        {
            return _repository.All<Solution>().Where(x => x.PuzzleId == puzzleId).ToList();
        }

        public IEnumerable<Vote> GetVotes(int puzzleId)
        {
            return _repository.All<Vote>().Where(x => x.PuzzleId == puzzleId).ToList();
        }

        public IPagination<SolutionProfile> GetSolutionsByUserId(int userId, SolutionSortType sortType, int page, int pageSize)
        {
            var groupedSolutions = _repository.All<SolutionProfile>().ByUserId(userId).ToList().GroupBy(x => x.PuzzleId);

            var solutions = new List<SolutionProfile>();
            foreach (var ig in groupedSolutions)
            {
                solutions.Add(ig.OrderByDescending(x => x.PointsAwarded).First());
            }

            switch (sortType)
            {
                case SolutionSortType.Newest:
                    solutions = solutions.OrderByDescending(x => x.DateCreated).ToList();
                    break;
                case SolutionSortType.Steps:
                    solutions = solutions.OrderBy(x => x.StepCount).ToList();
                    break;
                case SolutionSortType.PointsAwarded:
                    solutions = solutions.OrderByDescending(x => x.PointsAwarded).ToList();
                    break;
            }

            return solutions.Skip((page - 1)*pageSize).Take(pageSize).AsCustomPagination(page, pageSize, solutions.Count);
        }

        public void AddThemesToPuzzle(int puzzleId, int userId, IEnumerable<string> themes)
        {
            if (themes.Count() == 0)
                return;

            if (themes.Count() > 5)
                throw new InvalidOperationException("Cannot add more than 5 themes to a puzzle");

            var puzzle = _repository.All<Puzzle>().ById(puzzleId);
            if (puzzle == null)
                throw new InvalidOperationException("Puzzle does not exist");

            var isRetagging = false;
            if (puzzle.Themes.Count() > 0)
                isRetagging = true;

            var newThemes = new List<string>();

            foreach (var themeName in themes)
            {
                var loweredThemeName = themeName.ToLowerInvariant();
                var theme = _repository.All<Theme>().ByName(loweredThemeName);
                if (theme == null)
                {
                    theme = new Theme
                    {
                        DateCreated = DateTime.Now,
                        Name = loweredThemeName,
                        UserId = userId
                    };
                    _repository.Save(theme);
                }

                newThemes.Add(theme.Name);
            }
            puzzle.Themes = newThemes;
            _repository.Save(puzzle);

            if (isRetagging)
                _repository.Save(new ActionItem
                                     {
                                         Action = ActionType.ReTaggedPuzzle,
                                         AffectedUserId = puzzle.CreatedById,
                                         DateCreated = DateTime.Now,
                                         PuzzleId = puzzle.Id,
                                         UserId = _authenticationService.CurrentUserId
                                     });

        }

        public IPagination<Theme> GetThemes(int pageSize, int page, ThemeSortType sortType)
        {
            IEnumerable<Theme> themes = null;

            switch (sortType)
            {
                case ThemeSortType.Popular:
                    themes = _repository.All<Theme>().Where(x => x.Count > 0).OrderByDescending(x => x.Count).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case ThemeSortType.Alphabetical:
                    themes = _repository.All<Theme>().Where(x => x.Count > 0).OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
            }

            return new CustomPagination<Theme>(themes, page, pageSize, _repository.All<Theme>().Count());
        }

		public void UpdateThemeCount()
		{
			using (var connection = new SqlConnection(Settings.WikipediaMazeConnection))
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = "EXEC UpdateThemeCount";
					command.ExecuteNonQuery();
				}
			}
		}
		#endregion
	}
}
