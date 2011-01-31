using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwitterLib;
using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services
{
    public class MessengerService : IMessengerService
    {

        #region Fields

        private delegate void SendMessageDelegate(string text);
        private readonly IServiceApi _twitter;
        private readonly SendMessageDelegate _messageSender;
        private readonly UrlShorteningService _urlShortneningService;
        private readonly IRepository _repository;
        #endregion

        #region Constructors

        public MessengerService(IRepository repository)
        {
            _twitter = new TwitterNet(Settings.TwitterUserName, TwitterNet.ToSecureString(Settings.TwitterPassword));
            _twitter.TwitterServerUrl = Settings.TwitterServiceUrl;
            _messageSender = new SendMessageDelegate(SendMessage);
            _urlShortneningService = new UrlShorteningService(ShorteningService.Bitly);
            _repository = repository;
        }

        #endregion

        #region ITweetService Members

        public void SendPuzzleCreatedMessage(int puzzleId)
        {
            Puzzle puzzle;

            using (_repository.OpenSession())
            {
                puzzle = _repository.All<Puzzle>().ById(puzzleId);
            }
                if (puzzle == null) return;

                var url = GetPuzzleUrl(puzzle.Id);

                var userName = GetTwitterUserName(puzzle.User);

                var msg = "New puzzle by {0} {1} - {2} to {3} #wikipediamaze".ToFormat(userName, url, puzzle.StartTopic.FormatTopic(), puzzle.EndTopic.FormatTopic());
            
            _messageSender.BeginInvoke(msg, null, null); 
        }
        public void SendNewPuzzleLeaderMessage(int userId, int stepCount, int puzzleId)
        {
            Puzzle puzzle;
            WikipediaMaze.Core.User user;

            using (_repository.OpenSession())
            {
                user = _repository.All<WikipediaMaze.Core.User>().ById(userId);
                puzzle = _repository.All<Puzzle>().ById(puzzleId);
            }

            if (puzzle == null || user == null) return;

            var userName = GetTwitterUserName(user);

            var url = GetPuzzleUrl(puzzle.Id);
            var msg = "Player {0} now leading {1} - {2} to {3} in {4} steps. Can you beat it? #wikipediamaze".ToFormat(userName, url, puzzle.StartTopic.FormatTopic(), puzzle.EndTopic.FormatTopic(), stepCount.ToInvariantString());

            _messageSender.BeginInvoke(msg, null, null);
        }
        public void TweetSolution(int solutionId)
        {
            Solution solution;
            WikipediaMaze.Core.User user;
            Puzzle puzzle;

            using (_repository.OpenSession())
            {
                solution = _repository.All<Solution>().ById(solutionId);
                if (solution == null) return;

                user = _repository.All<WikipediaMaze.Core.User>().ById(solution.UserId);
                if (user == null) return;

                puzzle = _repository.All<Puzzle>().ById(solution.PuzzleId);
            }

            var userName = GetTwitterUserName(user);

            var url = GetPuzzleUrl(solution.PuzzleId);

            var msg = "I just solved the puzzle {0} - {1} to {2} in {3} steps. Can you beat it? #wikipediamaze".ToFormat(url, puzzle.StartTopic.FormatTopic(), puzzle.EndTopic.FormatTopic(), solution.StepCount.ToInvariantString());

            _messageSender.BeginInvoke(msg, null, null);
        }

#if HOMETEST || DEBUG

        private void SendMessage(string messageText)
        {
            messageText = _urlShortneningService.ShrinkUrls(messageText);
            _twitter.SendMessage("micahlmartin", messageText);
        }

#else
     
        private void SendMessage(string messageText)
        {
            messageText = _urlShortneningService.ShrinkUrls(messageText);
            _twitter.AddTweet(messageText);
        }

#endif

        private string GetTwitterUserName(WikipediaMaze.Core.User user)
        {
            return !string.IsNullOrEmpty(user.TwitterUserName) ?
                    "@{0}".ToFormat(user.TwitterUserName) :
                    user.DisplayName;
        }
        private static string GetPuzzleUrl(int puzzleId)
        {
            return "{0}/puzzles/{1}".ToFormat(Settings.Host, puzzleId);
        }
        #endregion
    }
}
