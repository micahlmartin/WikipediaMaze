using System;
using System.IO;
using MvcContrib.Filters;
using WikipediaMaze.App;
using WikipediaMaze.Controllers.SubControllers;
using WikipediaMaze.Core;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcContrib.Pagination;
using WikipediaMaze.Core.Mvc;
using WikipediaMaze.Services;
using System.Web.Routing;

namespace WikipediaMaze.Web.Controllers
{
//    [HandleErrorWithElmah]
//    [SubControllerActionToViewData]
//    public class FeedsController : Controller
//    {
//        #region Fields

//        private IPuzzleService _puzzleService;
//        private IAccountService _accountService;

//        #endregion

//        #region Contructors

//        public FeedsController(IPuzzleService puzzleService, IAccountService accountService)
//        {
//            _puzzleService = puzzleService;
//            _accountService = accountService;
//        }

//        #endregion

//        public ActionResult Index(HeaderInfoController headerInfoController)
//        {
//            return View();
//        }

//        public ActionResult Puzzles(PuzzleSortType? sortType, FeedFormat? format)
//        {
//            format = format ?? FeedFormat.Atom;
//            sortType = sortType ?? PuzzleSortType.Newest;

//            //Get sorted puzzles
//            IPagination<Puzzle> puzzles = _puzzleService.GetPuzzles(sortType.Value, 1, 50);

//            return format.Value == FeedFormat.Json ?
//                                   GetPuzzlesJsonFeed(puzzles) :
//                                   GetPuzzlesRssFeed(format, sortType, puzzles);
//        }
//        public ActionResult Themed(string themes, PuzzleSortType? sortType, FeedFormat? format)
//        {
//            format = format ?? FeedFormat.Atom;
//            sortType = sortType ?? PuzzleSortType.Newest;

//            //Get sorted puzzles
//            var themeList = Theme.GetThemesFromString(themes);
//            IPagination<Puzzle> puzzles = _puzzleService.GetPuzzles(sortType.Value, 1, 50, themeList);

//            return format.Value == FeedFormat.Json ?
//                    GetPuzzlesJsonFeed(puzzles) :
//                    GetThemedPuzzlesRssFeed(format, sortType, themeList, puzzles);
//        }
//        public ActionResult PlayerPuzzles(int id, PuzzleSortType? sortType, FeedFormat? format)
//        {
//            format = format ?? FeedFormat.Atom;
//            sortType = sortType ?? PuzzleSortType.Newest;

//            //Get sorted puzzles
//            IEnumerable<Puzzle> puzzles = _puzzleService.GetPuzzlesByUserId(id, sortType.Value, 1, 10000).Where(x => x.IsVerified);

//            return format.Value == FeedFormat.Json ?
//                                   GetPuzzlesJsonFeed(puzzles) :
//                                   GetPuzzlesRssFeed(format, sortType, puzzles);
//        }
//        public ActionResult Players(PlayerSortType? sortType, FeedFormat? format)
//        {
//            sortType = sortType ?? PlayerSortType.Reputation;
//            format = format ?? FeedFormat.Atom;

//            IEnumerable<User> players = _accountService.GetAllUsers(sortType.Value);
//            return format.Value == FeedFormat.Json ?
//                GetPlayersJsonFeed(players) :
//                GetPlayersRssFeed(sortType, format, players);
//        }
//        public ActionResult PlayerSolutions(int id, string userName, SolutionSortType? sortType, FeedFormat? format)
//        {
//            sortType = sortType ?? SolutionSortType.Newest;
//            format = format ?? FeedFormat.Atom;

//            User player = _accountService.GetUserById(id);
//            if (player == null)
//                throw new FileNotFoundException();

//            IPagination<Solution> solutions = _puzzleService.GetSolutionsByUserId(id, sortType.Value, 1, 10000);

//            return format.Value == FeedFormat.Json ?
//                GetSolutionJsonFeed(solutions) :
//                GetSolutionRssFeed(sortType, format, player, solutions);
//        }
//        private ActionResult GetSolutionJsonFeed(IEnumerable<Solution> solutions)
//        {
//            var jsonSolutions =
//                solutions.Select(
//                    x =>
//                    new
//                        {
//                            puzzleId = x.PuzzleId,
//                            pointsAwarded = x.PointsAwarded,
//                            stepCount = x.StepCount,
//                            dateCreated = x.DateCreated,
//                            startTopic = x.StartTopic.FormatTopic(),
//                            endTopic = x.EndTopic.FormatTopic()
//                        });

//            return Json(jsonSolutions, JsonRequestBehavior.AllowGet);
//        }
//        private ActionResult GetSolutionRssFeed(SolutionSortType? sortType, FeedFormat? format, User player, IEnumerable<Solution> solutions)
//        {
//            var feed = new SyndicationFeed
//                           {
//                               Title = new TextSyndicationContent(GetPlayerSolutionsTitle(sortType.Value)),
//                               Description = new TextSyndicationContent("Solutions by '{0}' from WikipediaMaze.com".ToFormat(player.DisplayName)),
//                               Id = Request.Url.ToString(),
//                               LastUpdatedTime = new DateTimeOffset(DateTime.Now)
//                           };
//            feed.Links.Add(SyndicationLink.CreateSelfLink(new Uri(Request.Url.ToString())));
//            feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetUserLink(player))));

//            var items = new List<SyndicationItem>();
//            feed.Items = items;

//            foreach (var solution in solutions)
//            {
//                items.Add(GetSyndicationItem(solution, player));
//            }

//            BaseFeedActionResult result = FeedActionResultFactory.GetFeedActionResult(format.Value);
//            result.Feed = feed;
//            return result;
//        }
//        private ActionResult GetPlayersJsonFeed(IEnumerable<User> players)
//        {
//            var jsonPlayers =
//                players.Select(
//                    x =>
//                    new
//                        {
//                            id = x.Id,
//                            lastVisit = x.LastVisit,
//                            reputation = x.Reputation,
//                            bronzeBadges =
//                        x.Badges.Where(y => y.Level == BadgeLevel.Bronze).Select(y => y.Name).Distinct(),
//                            bronzeBadgeCount = x.Badges.Where(y => y.Level == BadgeLevel.Bronze).Count(),
//                            silverBadges =
//                        x.Badges.Where(y => y.Level == BadgeLevel.Silver).Select(y => y.Name).Distinct(),
//                            silverBadgeCount = x.Badges.Where(y => y.Level == BadgeLevel.Silver).Count(),
//                            goldBadges = x.Badges.Where(y => y.Level == BadgeLevel.Gold).Select(y => y.Name).Distinct(),
//                            goldBadgeCount = x.Badges.Where(y => y.Level == BadgeLevel.Gold).Count(),
//                            birthDate = x.BirthDate,
//                            location = x.Location,
//                            photoUrl = x.GetGravatarUrl(null),
//                            displayName = x.DisplayName,
//                            leadingPuzzleCount = x.LeadingPuzzleCount,
//                            twitterUserName = x.TwitterUserName
//                        });
//            return Json(jsonPlayers, JsonRequestBehavior.AllowGet);
//        }
//        private ActionResult GetPlayersRssFeed(PlayerSortType? sortType, FeedFormat? format, IEnumerable<User> players)
//        {
//            var feed = new SyndicationFeed
//                           {
//                               Title = new TextSyndicationContent(GetPlayersPageTitle(sortType.Value)),
//                               Description = new TextSyndicationContent("All players from WikipediaMaze.com"),
//                               Id = Request.Url.ToString(),
//                               LastUpdatedTime = new DateTimeOffset(DateTime.Now)
//                           };
//            feed.Links.Add(SyndicationLink.CreateSelfLink(new Uri(Request.Url.ToString())));
//            feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetAlternatePlayerListLink(sortType.Value))));

//            var items = new List<SyndicationItem>();
//            feed.Items = items;

//            foreach (var player in players)
//            {
//                items.Add(GetSyndicationItem(player));
//            }

//            BaseFeedActionResult result = FeedActionResultFactory.GetFeedActionResult(format.Value);
//            result.Feed = feed;
//            return result;
//        }
//        private ActionResult GetPuzzlesRssFeed(FeedFormat? format, PuzzleSortType? sortType, IEnumerable<Puzzle> puzzles)
//        {
//            SyndicationFeed feed = GetPuzzlesFeed(sortType, puzzles);

//            BaseFeedActionResult result = FeedActionResultFactory.GetFeedActionResult(format.Value);
//            result.Feed = feed;
//            return result;
//        }
//        private ActionResult GetPuzzlesJsonFeed(IEnumerable<Puzzle> puzzles)
//        {
//            var jsonPuzzles = puzzles.Select(x => new
//                                    {
//                                        id = x.Id,
//                                        startTopic = x.StartTopic.FormatTopic(),
//                                        endTopic = x.EndTopic.FormatTopic(),
//                                        creatorId = x.User.Id,
//                                        dateCreated = x.DateCreated,
//                                        level = x.Level,
//                                        voteCount = x.VoteCount,
//                                        themes = x.Themes,
//                                        solutionCount = x.SolutionCount,
//                                        leadingPlayerId = x.LeaderId
//                                    });
//            return Json(jsonPuzzles, JsonRequestBehavior.AllowGet);
//        }
//        private ActionResult GetThemedPuzzlesRssFeed(FeedFormat? format, PuzzleSortType? sortType, IEnumerable<string> themeList, IEnumerable<Puzzle> puzzles)
//        {
//            var feed = new SyndicationFeed
//                           {
//                               Title = new TextSyndicationContent(GetThemedPageTitle(sortType.Value, themeList)),
//                               Description = new TextSyndicationContent("First 50 puzzles from WikipediaMaze.com"),
//                               Id = Request.Url.ToString(),
//                               LastUpdatedTime = new DateTimeOffset(DateTime.Now)
//                           };
//            feed.Links.Add(SyndicationLink.CreateSelfLink(new Uri(Request.Url.ToString())));
//            feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetAlternateThemedLink(sortType.Value, themeList))));

//            foreach (var theme in themeList)
//            {
//                var category = new SyndicationCategory(theme);
//                feed.Categories.Add(category);
//            }

//            var items = new List<SyndicationItem>();
//            feed.Items = items;

//            foreach (var puzzle in puzzles)
//            {
//                items.Add(GetSyndicationItem(puzzle));
//            }

//            BaseFeedActionResult result = FeedActionResultFactory.GetFeedActionResult(format.Value);
//            result.Feed = feed;
//            return result;
//        }
//        private SyndicationFeed GetPuzzlesFeed(PuzzleSortType? sortType, IEnumerable<Puzzle> puzzles)
//        {
//            var feed = new SyndicationFeed
//            {
//                Title = new TextSyndicationContent(GetPuzzlesPageTitle(sortType.Value)),
//                Description = new TextSyndicationContent("First 50 puzzles from WikipediaMaze.com"),
//                Id = Request.Url.ToString(),
//                LastUpdatedTime = new DateTimeOffset(DateTime.Now)
//            };
//            feed.Links.Add(SyndicationLink.CreateSelfLink(new Uri(Request.Url.ToString())));
//            feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetAlternatePuzzleListLink(sortType.Value))));

//            var items = new List<SyndicationItem>();
//            feed.Items = items;

//            foreach (var puzzle in puzzles)
//            {
//                items.Add(GetSyndicationItem(puzzle));
//            }
//            return feed;
//        }
//        private static string GetPlayerSolutionsTitle(SolutionSortType sortType)
//        {
//            var title = "";
//            switch (sortType)
//            {
//                case SolutionSortType.Newest:
//                    title = "Newest";
//                    break;
//                case SolutionSortType.PointsAwarded:
//                    title = "Highest Scoring";
//                    break;
//                case SolutionSortType.Steps:
//                    title = "Quickest";
//                    break;
//                default:
//                    throw new NotImplementedException("Need to handle sorting for type {0}".ToFormat(sortType));
//            }
//            return title += " Solutions";
//        }
//        private static SyndicationItem GetSyndicationItem(Solution solution, User player)
//        {
//            var item = new SyndicationItem();
//            item.Id = GetUserLink(player);
//            item.Title = new TextSyndicationContent("{0} to {1}".ToFormat(solution.StartTopic.FormatTopic(), solution.EndTopic.FormatTopic()));
//            item.Authors.Add(new SyndicationPerson { Name = player.DisplayName, Uri = GetUserLink(player) });
//            item.Summary = new TextSyndicationContent(GetSolutionSummary(solution), TextSyndicationContentKind.Html);
//            item.PublishDate = solution.DateCreated;
//            item.LastUpdatedTime = solution.DateCreated;
//            item.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetPuzzleLink(solution.PuzzleId))));
//            return item;
//        }
//        private static string GetSolutionSummary(Solution solution)
//        {
//            return
//                @"
//                <table>
//                    <tr>
//                        <td>From:</td>
//                        <td><a href=""http://www.wikipediamaze.com/wiki/{0}"" target=""_blank"">{1}</a></td>
//                    </tr>
//                    <tr>
//                        <td>To:</td>
//                        <td><a href=""http://www.wikipediamaze.com/wiki/{2}"" target=""_blank"">{3}</a></td>
//                    </tr>
//                    <tr>
//                        <td>Steps:</td>
//                        <td>{4}</td>
//                    </tr>
//                    <tr>
//                        <td>Points:</td>
//                        <td>{5}</td>
//                    </tr>       
//                </table>
//                ".ToFormat(solution.StartTopic.FormatTitleForUrl(), 
//                           solution.StartTopic.FormatTopic(), 
//                           solution.EndTopic.FormatTitleForUrl(), 
//                           solution.EndTopic.FormatTopic(),
//                           solution.StepCount, 
//                           solution.PointsAwarded);
//        }
//        private static string GetPlayersPageTitle(PlayerSortType sortType)
//        {
//            switch (sortType)
//            {
//                case PlayerSortType.Name:
//                    return "Players Listed Alphabetically";
//                case PlayerSortType.Newest:
//                    return "Newest Players";
//                case PlayerSortType.Oldest:
//                    return "Longest Playing Players";
//                case PlayerSortType.Reputation:
//                    return "Highest Scoring Players";
//                default:
//                    throw new NotImplementedException("Need to handle sorting for type {0}".ToFormat(sortType));
//            }
//        }
//        private static string GetAlternatePlayerListLink(PlayerSortType sortType)
//        {
//            return "http://www.wikipediamaze.com/players?sortType=".ToFormat(Enum.Format(typeof(PlayerSortType), sortType, "g"));
//        }
//        private static SyndicationItem GetSyndicationItem(User player)
//        {
//            var item = new SyndicationItem();
//            item.Id = GetUserLink(player);
//            item.Title = new TextSyndicationContent(player.DisplayName);
//            item.Authors.Add(new SyndicationPerson { Name = player.DisplayName, Uri = GetUserLink(player) });
//            item.Summary = new TextSyndicationContent(GetPlayerSummary(player), TextSyndicationContentKind.Html);
//            item.PublishDate = player.DateCreated;
//            item.LastUpdatedTime = player.DateCreated;
//            item.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetUserLink(player))));
//            return item;
//        }
//        private static string GetPlayerSummary(User player)
//        {
//            return
//                @"
//                <table>
//                    <tr>
//                        <td>Reputation:</td>
//                        <td>{0}</td>
//                    </tr>
//                    <tr>
//                        <td>Leading:</td>
//                        <td>{1}</td>
//                    </tr>
//                    <tr>
//                        <td>Bronze Badges:</td>
//                        <td>{2}</td>
//                    </tr>
//                    <tr>
//                        <td>Silver Badges:</td>
//                        <td>{3}</td>
//                    </tr>
//                    <tr>
//                        <td>Gold Badges:</td>
//                        <td>{4}</td>
//                    </tr>          
//                </table>
//                ".ToFormat(player.Reputation, player.LeadingPuzzleCount, 
//                 player.Badges.Select(x => x.Level == BadgeLevel.Bronze).Count(),
//                 player.Badges.Select(x => x.Level == BadgeLevel.Silver).Count(),
//                 player.Badges.Select(x => x.Level == BadgeLevel.Gold).Count());
//        }
//        private static SyndicationItem GetSyndicationItem(Puzzle puzzle)
//        {
//            var item = new SyndicationItem();
//            item.Id = GetPuzzleLink(puzzle);
//            item.Title = new TextSyndicationContent(GetPuzzleTitle(puzzle));
//            item.Authors.Add(new SyndicationPerson { Name = puzzle.User.DisplayName, Uri = GetUserLink(puzzle.User) });
//            item.Summary = new TextSyndicationContent(GetPuzzleSummary(puzzle), TextSyndicationContentKind.Html);
//            item.PublishDate = puzzle.DateCreated;
//            item.LastUpdatedTime = puzzle.DateCreated;
//            item.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetPuzzleLink(puzzle))));
//            foreach (var theme in puzzle.Themes)
//            {
//                var category = new SyndicationCategory(theme);
//                item.Categories.Add(category);
//            }

//            return item;
//        }
//        private static string GetPuzzlesPageTitle(PuzzleSortType sortType)
//        {
//            var sb = new StringBuilder();
//            switch (sortType)
//            {
//                case PuzzleSortType.Level:
//                    sb.Append("Hardest");
//                    break;
//                case PuzzleSortType.Newest:
//                    sb.Append("Newest");
//                    break;
//                case PuzzleSortType.Solutions:
//                    sb.Append("Most Played");
//                    break;
//                case PuzzleSortType.Votes:
//                    sb.Append("Highest Voted");
//                    break;
//                default:
//                    throw new NotImplementedException("Need to handle sorting for type {0}".ToFormat(sortType));
//            }
//            sb.Append(" Puzzles - Wikipedia Maze");
//            return sb.ToString();
//        }
//        private static string GetThemedPageTitle(PuzzleSortType sortType, IEnumerable<string> themes)
//        {
//            var sb = new StringBuilder();
//            switch (sortType)
//            {
//                case PuzzleSortType.Level:
//                    sb.Append("Hardest");
//                    break;
//                case PuzzleSortType.Newest:
//                    sb.Append("Newest");
//                    break;
//                case PuzzleSortType.Solutions:
//                    sb.Append("Most Played");
//                    break;
//                case PuzzleSortType.Votes:
//                    sb.Append("Highest Voted");
//                    break;
//                default:
//                    throw new NotImplementedException("Need to handle sorting for type {0}".ToFormat(sortType));
//            }
//            sb.Append(" Puzzles Themed ");
//            foreach (var theme in themes)
//            {
//                sb.AppendFormat("{0}, ", theme);
//            }

//            return sb.ToString().Trim(' ', ',') + " - Wikipedia Maze";
//        }
//        private static string GetAlternateThemedLink(PuzzleSortType sortType, IEnumerable<string> themes)
//        {
//            var sb = new StringBuilder("http://www.wikipediamaze.com/puzzles/themed?sortType=".ToFormat(Enum.Format(typeof(PuzzleSortType), sortType, "g")));
//            sb.Append("themes=");
//            foreach (var theme in themes)
//            {
//                sb.AppendFormat("{0},", theme);
//            }
//            return sb.ToString().Trim(',');
//        }
//        private static string GetAlternatePuzzleListLink(PuzzleSortType sortType)
//        {
//            return "http://www.wikipediamaze.com/puzzles?sortType=".ToFormat(Enum.Format(typeof(PuzzleSortType), sortType, "g"));
//        }
//        private static string GetUserLink(User user)
//        {
//            return "http://www.wikipediamaze.com/players/{0}/{1}".ToFormat(user.Id, user.DisplayName.FormatTitleForUrl());
//        }
//        private static string GetPuzzleLink(Puzzle puzzle)
//        {
//            return GetPuzzleLink(puzzle.Id);
//        }
//        private static string GetPuzzleLink(int puzzleId)
//        {
//            return "http://www.wikipediamaze.com/puzzles/{0}".ToFormat(puzzleId);
//        }

//        private static string GetPuzzleTitle(Puzzle puzzle)
//        {
//            return "{0} To {1}".ToFormat(puzzle.StartTopic.FormatTopic(), puzzle.EndTopic.FormatTopic());
//        }
//        private static string GetPuzzleSummary(Puzzle puzzle)
//        {
//            return (@"
//                <table>
//                    <tr>
//                        <td>Votes:</td>
//                        <td>{0}</td>
//                    </tr>
//                    <tr>
//                        <td>Level:</td>
//                        <td>{1}</td>
//                    </tr>
//                    <tr>
//                        <td>Solutions:</td>
//                        <td>{2}</td>
//                    </tr>
//                    <tr>
//                        <td><a href=""http://www.wikipediamaze.com/game/start/{3}"">Solve</a></td>
//                        <td><a href=""http://www.wikipediamaze.com/puzzles/{3}"">View</a></td>
//                    </tr>
//                </table>".ToFormat(puzzle.VoteCount, puzzle.Level, puzzle.SolutionCount, puzzle.Id));
//        }
//    }
}