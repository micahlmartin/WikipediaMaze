using System;
using WikipediaMaze.App;
using WikipediaMaze.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikipediaMaze.ViewModels
{
    public class PuzzleViewModel
    {
        private readonly Puzzle _puzzle;
        private readonly VoteType _voteType;
        private UserInfoViewModel _userInfo;
        private string _shareThisPuzzleMessage;
        private string _shareThisSolutionMessage;

        public PuzzleViewModel(Puzzle puzzle, VoteType voteType, Solution latestSolution, Solution bestSolution, int userSolutionCount, IEnumerable<SolutionViewModel> puzzleLeaderBoard, bool isCreator, bool isLeading)
        {
            _puzzle = puzzle;   
            _voteType = voteType;
            LatestSolution = latestSolution;
            BestSolution = bestSolution;
            UserSolutionCount = userSolutionCount;
            PuzzleLeaderBoard = puzzleLeaderBoard;
            IsCreator = isCreator;
            Themes = puzzle.Themes;
            IsLeading = isLeading;
        }

        public int PuzzleId
        {
            get { return _puzzle.Id; }
        }
        public string StartTopic
        {
            get { return _puzzle.StartTopic.FormatTopic(); }
        }
        public string EndTopic
        {
            get
            {
                return _puzzle.EndTopic.FormatTopic();
            }
        }
        public int PuzzleLevel
        {
            get
            {
                return _puzzle.Level;
            }
        }
        public VoteType UserVote
        {
            get { return _voteType; }
        }
        public int VoteCount
        {
            get { return _puzzle.VoteCount; }
        }
        public int Level
        {
            get { return _puzzle.Level; }
        }
        public int SolutionCount
        {
            get { return _puzzle.SolutionCount; }
        }
        public DateTime DateCreated
        {
            get { return _puzzle.DateCreated; }
        }
        public UserInfoViewModel UserInfo
        {
            get
            {
                if(_userInfo == null)
                    _userInfo = new UserInfoViewModel(_puzzle.User);

                return _userInfo;
            }   
        }
        public bool HasSolution
        {
            get { return LatestSolution != null; }
        }
        public bool IsVerified
        {
            get
            {
                return _puzzle.IsVerified;
            }
        }
        public Solution LatestSolution { get; private set; }
        public Solution BestSolution { get; private set; }
        public int UserSolutionCount { get; private set; }
        public bool IsCreator { get; private set; }
        public IEnumerable<string> Themes { get; private set; } 
        public IEnumerable<SolutionViewModel> PuzzleLeaderBoard { get; private set; }
        public bool IsLeading { get; private set; }

        public string ShareThisPuzzleMessage
        {
            get
            {
                if (_shareThisPuzzleMessage == null)
                {
                    var sb = new StringBuilder();

                    sb.Append("Check out this new puzzle ".UrlEncode());
                    sb.Append("http://www.wikipediamaze.com/puzzles/{0}".ToFormat(PuzzleId));
                    sb.Append(" {0} to {1}. Can you solve it".ToFormat(StartTopic.FormatTopic(), EndTopic.FormatTopic()).UrlEncode());
                    sb.Append("?");

                    _shareThisPuzzleMessage = sb.ToString();
                }

                return _shareThisPuzzleMessage;
            }
        }

        public string ShareThisSolutionMessage
        {
            get
            {
                if (_shareThisSolutionMessage == null && BestSolution != null)
                {
                    var sb = new StringBuilder();
                    sb.Append("Solved the puzzle ".UrlEncode());
                    sb.Append("http://www.wikipediamaze.com/puzzles/{0}".ToFormat(_puzzle.Id));
                    sb.Append(" {0} to {1} in {2} steps. Can you beat it?".ToFormat(_puzzle.StartTopic.FormatTopic(), _puzzle.EndTopic.FormatTopic(), BestSolution.StepCount).UrlEncode());

                    _shareThisSolutionMessage = sb.ToString();
                }

                return _shareThisSolutionMessage;
            }
        }
    }
}