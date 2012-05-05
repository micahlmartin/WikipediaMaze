using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using log4net;
using MvcContrib.Filters;
using MvcContrib.Pagination;
using WikipediaMaze.App;
using WikipediaMaze.Controllers.SubControllers;
using WikipediaMaze.Core;
using WikipediaMaze.Services;
using WikipediaMaze.ViewModels;
using WikipediaMaze.Web;
using WikipediaMaze.Services.Interfaces;
using WikipediaMaze.Core.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;

namespace WikipediaMaze.ViewModels
{
    public class UserProfileViewModel
    {
        private readonly User _user;
        private readonly int _currentUserId;
        private readonly DateTime? _lastActivity;
        public UserProfileViewModel(User user, int currentUserId, DateTime? lastActivity)
        {
            _user = user;
            _currentUserId = currentUserId;
            _lastActivity = lastActivity;

            var badgeViewModels = new List<BadgeViewModel>();
            var groupedBadges = user.Badges.GroupBy(x => x.Name);
            foreach (var badge in groupedBadges)
            {
                badgeViewModels.Add(new BadgeViewModel(badge));
            }

            Badges = badgeViewModels;
        }

        public string GetGravatarUrl(int size)
        {
            return _user.GetGravatarUrl(size);
        }
        public string UserName
        {
            get
            {
                return _user.DisplayName;
            }
        }
        public int UserId
        {
            get
            {
                return _user.Id;
            }
        }
        public int Reputation
        {
            get { return _user.Reputation; }
        }
        public int GoldBadgeCount
        {
            get { return _user.Badges.Where(x => x.Level == BadgeLevel.Gold).Count(); }
        }
        public int SilverBadgeCount
        {
            get { return _user.Badges.Where(x => x.Level == BadgeLevel.Silver).Count(); }
        }
        public int BronzeBadgeCount
        {
            get { return _user.Badges.Where(x => x.Level == BadgeLevel.Bronze).Count(); }
        }
        public DateTime LastSeen
        {
            get { return _lastActivity ?? _user.LastVisit; }
        }
        public DateTime Joined
        {
            get { return _user.DateCreated; }
        }
        public bool IsUser
        {
            get
            {
                return UserId == _currentUserId;
            }
        }
        public string Age
        {
            get
            {
                return _user.BirthDate.HasValue ? _user.BirthDate.Value.FormatAge().ToString(System.Globalization.CultureInfo.InvariantCulture) : string.Empty;
            }
        }
        public string Location
        {
            get { return _user.Location; }
        }
        public string OpenId
        {
            get { return _user.OpenIdentifiers.First().Identifier; }
        }
        public string TwitterUserName
        {
            get { return _user.TwitterUserName; }
        }
        public int LeadingPuzzleCount
        {
            get{return _user.LeadingPuzzleCount;}
        }
        public IEnumerable<BadgeViewModel> Badges { get; private set; }
    }
    public class UserProfilePuzzleViewModel
    {
        private readonly Puzzle _puzzle;
        private readonly VoteType _userVote;

        public UserProfilePuzzleViewModel(Puzzle puzzle, VoteType userVote, bool isAuthenticated, int currentUserId)
        {
            IsAuthenticated = isAuthenticated;
            CurrentUserId = currentUserId;
            _puzzle = puzzle;
            _userVote = userVote;
            Themes = puzzle.Themes;
        }
        public IEnumerable<string> Themes { get; private set; }
        public int PuzzleId
        {
            get { return _puzzle.Id; }
        }
        public string StartTopic
        {
            get { return _puzzle.StartTopic; }
        }
        public string EndTopic
        {
            get
            {
                return _puzzle.EndTopic;
            }
        }
        public int Level
        {
            get
            {
                return _puzzle.Level;
            }
        }
        public int VoteCount
        {
            get { return _puzzle.VoteCount; }
        }
        public int SolutionCount
        {
            get { return _puzzle.SolutionCount; }
        }
        public DateTime DateCreated
        {
            get { return _puzzle.DateCreated; }
        }
        public bool IsVerified
        {
            get
            {
                return _puzzle.IsVerified;
            }
        }
        public VoteType UserVote
        {
            get { return _userVote; }
        }
        public bool IsAuthenticated { get; private set; }
        public int CurrentUserId { get; private set; }
        public bool CanShowPuzzle
        {
            get
            {
                //If the puzzle is verified everyone can see it.
                if(IsVerified)
                    return true;

                //Only authenticated users can see unverified puzzles
                if(!IsAuthenticated)
                    return false;

                return _puzzle.User.Id == CurrentUserId;
            }
        }
    }
    public class UserProfileListViewModel
    {
        public UserProfileListViewModel(IPagination<UserInfoViewModel> users, PlayerSortType sortType)
        {
            Users = users;
            SortType = sortType;
        }

        public IPagination<UserInfoViewModel> Users { get; private set; }
        public PlayerSortType SortType { get; private set; }
        public string PageTitle
        {
            get
            {
                switch (SortType)
                {
                    case PlayerSortType.Name:
                        return "Players by Name";
                    case PlayerSortType.Newest:
                        return "Newest Players";
                    case PlayerSortType.Oldest:
                        return "Players by Date Joined";
                    case PlayerSortType.Reputation:
                        return "Players by Reputation";
                    default:
                        return "Players";
                }
            }
        }
    }
    public class UserProfileUpdateViewModel
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string RealName { get; set; }
        public string Location { get; set; }
        public DateTime? Birthday { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string TwitterUserName { get; set; }
        public string GetGravatar(int size)
        {
            if (!string.IsNullOrEmpty(Email))
                return Email.AsGravatarUrl(size);

            return (DisplayName + "@wikipediamaze.com").AsGravatarUrl(size);
        }
    }
    public class BadgeViewModel
    {
        public BadgeViewModel(IEnumerable<UserBadgeInfo> badges)
        {
            var badge = badges.ElementAt(0);
            BadgeCount = badges.Count();
            BadgeName = badge.Name;
            BadgeLevel = badge.Level.ToString().ToLowerInvariant();
            BadgeDescription = badge.Description;
        }
        public int BadgeCount { get; private set; }
        public string BadgeName { get; private set; }
        public string BadgeLevel { get; private set; }
        public string BadgeDescription { get; private set; }
    }
    public class UserProfilePuzzleListViewModel
    {

        public UserProfilePuzzleListViewModel(IPagination<UserProfilePuzzleViewModel> puzzles, PuzzleSortType sortType, int profileId)
        {
            Puzzles = puzzles;
            SortType = sortType;
            ProfileId = profileId;
        }

        public IPagination<UserProfilePuzzleViewModel> Puzzles { get; private set; }
        public PuzzleSortType SortType { get; private set; }
        public int ProfileId { get; private set; }
    }
    public class UserProfileSolutionListViewModel
    {
        public UserProfileSolutionListViewModel(IPagination<Solution> solutions, SolutionSortType sortType, int profileId)
        {
            Solutions = solutions;
            SortType = sortType;
            ProfileId = profileId;
        }

        public IPagination<Solution> Solutions { get; private set; }
        public SolutionSortType SortType { get; private set; }
        public int ProfileId { get; private set; }
    }
}

namespace WikipediaMaze.Controllers
{
    [HandleErrorWithElmah]
    [SubControllerActionToViewData]
    public class PlayersController : Controller
    {
        #region Fields

        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IAccountService _accountService;
        private readonly IPuzzleService _puzzleService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IWebSnapshotService _snapshotService;
        #endregion

        #region Constructors

        public PlayersController(IAccountService accountService, IPuzzleService puzzleService, IAuthenticationService authenticationService, IWebSnapshotService snapshotService)
        {
            _accountService = accountService;
            _puzzleService = puzzleService;
            _authenticationService = authenticationService;
            _snapshotService = snapshotService;
        }

        #endregion

        public ActionResult Index(HeaderInfoController headerInfoController, PlayerSortType? sortType, int? page)
        {
            sortType = sortType ?? PlayerSortType.Reputation;
            page = page ?? 1;

            var users = _accountService.GetUsers(page.Value, 28, sortType.Value);
            var userVms = users.Select(x => new UserInfoViewModel(x)).AsCustomPagination(users.PageNumber, users.PageSize, users.TotalItems);
            return View(new UserProfileListViewModel(userVms, sortType.Value));
        }

        public ActionResult Display(HeaderInfoController headerInfoController, int id, string userName, PuzzleSortType? sortType, int? page, int? pageSize)
        {
            sortType = sortType ?? PuzzleSortType.Newest;
            page = page ?? 1;
            pageSize = pageSize ?? 10;

            var user = _accountService.GetUserById(id);
            
            if (user == null)
                return View("NotFound");

            var loggedInUserId = _authenticationService.CurrentUserId;
            var lastActivity = _accountService.GetLastActivityDate(user.Id);
            var profileVM = new UserProfileViewModel(user, loggedInUserId, lastActivity);

            return View("Display", profileVM);
        }

        public ActionResult Edit(HeaderInfoController headerInfoController, int id)
        {
            var user = _accountService.GetUserById(id);
            
            if (user == null)
                return View("NotFound");
            
            if (user.Id != _authenticationService.CurrentUserId)
                return View("Unauthorized");

            return View(new UserProfileUpdateViewModel {UserName = user.DisplayName, UserId = user.Id, Birthday = user.BirthDate, DisplayName = user.DisplayName, Email = user.Email, Location = user.Location, RealName = user.RealName, TwitterUserName = user.TwitterUserName });
        }

        /// <summary>
        /// Displays the puzzles on the user profile page.
        /// </summary>
        /// <param name="id">The id of the user who's profile is being displayed.</param>
        /// <param name="sortType">The id of the user who's profile is being displayed</param>
        /// <param name="page">The page number to show</param>
        /// <param name="pageSize">The size of the page to return.</param>
        public ActionResult UserDisplayPuzzles(int id, PuzzleSortType? sortType, int? page, int? pageSize)
        {
            sortType = sortType ?? PuzzleSortType.Newest;
            page = page ?? 1;
            pageSize = pageSize ?? 10;

            var loggedInUserId = _authenticationService.CurrentUserId;
            var user = _accountService.GetUserById(id);

            if (user == null)
                return View("NotFound");

            var puzzles = _puzzleService.GetPuzzlesByUserId(id, sortType.Value, page.Value, pageSize.Value);

            var puzzleVms = new List<UserProfilePuzzleViewModel>();
            foreach (var puzzle in puzzles)
            {
                var puzzleId = puzzle.Id;
                var userVote = puzzle.Votes.Where(x => x.UserId == loggedInUserId && x.PuzzleId == puzzleId).SingleOrDefault();
                var userVoteType = userVote == null ? VoteType.None : userVote.VoteType;
                puzzleVms.Add(new UserProfilePuzzleViewModel(puzzle, userVoteType, _authenticationService.IsAuthenticated, loggedInUserId));
            }

            var userProfilePuzzleVMs = new CustomPagination<UserProfilePuzzleViewModel>(puzzleVms, puzzles.PageNumber, puzzles.PageSize, puzzles.TotalItems);

            return PartialView(new UserProfilePuzzleListViewModel(userProfilePuzzleVMs, sortType.Value, user.Id));
        }

        /// <summary>
        /// Display the solutions on the user profile page.
        /// </summary>
        /// <param name="id">The id of the user who's profile is being displayed</param>
        /// <param name="sortType">The type of sort to apply to the solutions.</param>
        /// <param name="page">The page number to show</param>
        public ActionResult UserDisplaySolutions(int id, SolutionSortType? sortType, int? page)
        {
            sortType = sortType ?? SolutionSortType.Newest;
            page = page ?? 1;
            var pageSize = 30;

            IPagination<Solution> solutions = _puzzleService.GetSolutionsByUserId(id, sortType.Value, page.Value, pageSize);

            return PartialView(new UserProfileSolutionListViewModel(solutions, sortType.Value, id));
        }


        [OutputCache(Duration = 900)]
        public ActionResult FlairImage(int id, int? width)
        {
            width = width ?? 220;
            width = Math.Max(width.Value, 220);

            MemoryStream imageStream;

            var imageBytes = HttpRuntime.Cache[GetFLairCacheKey(id, width.Value)] as byte[];
            if (imageBytes != null)
            {
                imageStream = new MemoryStream(imageBytes);
            }
            else
            {
                imageStream = new MemoryStream();
                var url = Settings.Host + Url.Action("flairdetail", new { id = id, width = width.Value });
                var image = _snapshotService.GetSnapshot(url, 74, width.Value + 20);
                image.Save(imageStream, ImageFormat.Png);
                imageStream.Flush();
                imageStream.Position = 0;
                imageBytes = imageStream.ReadToEnd();
                HttpRuntime.Cache.Add(GetFLairCacheKey(id, width.Value), imageBytes, null, DateTime.Now.AddMinutes(Settings.FlairCacheMinutes), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                imageStream.Position = 0;
            }

            var result = new ImageResult(imageStream, "image/png");
            return result;
        }
        private const string FlairCacheKeyBase = "Flair/{0}/{1}";
        private static string GetFLairCacheKey(int id, int width)
        {
            return FlairCacheKeyBase.ToFormat(id, width);
        }

        public ActionResult FlairDetail(int id, int? width)
        {
            width = width ?? 220;
            width = Math.Max(width.Value, 220);
            var user = _accountService.GetUserById(id);
            var vm = new UserProfileViewModel(user, id, null);
            ViewBag.Width = width;
            return View(vm);
        }

        public ActionResult Flair(int id)
        {
            var user = _accountService.GetUserById(id);
            if (user == null)
                throw new FileNotFoundException();

            var userVM = new UserProfileViewModel(user, _authenticationService.CurrentUserId, null);
            ViewBag.CurrentUserId = userVM.UserId;
            ViewBag.CurrentUsername = userVM.UserName;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken(Salt = "#!)Su&$%!_s{")]
        public ActionResult Edit(HeaderInfoController headerInfoController, UserProfileUpdateViewModel userProfile)
        {
            var user = _accountService.GetUserById(userProfile.UserId);
            if (user == null)
                throw new System.IO.FileNotFoundException();

            user.BirthDate = userProfile.Birthday;
            user.DisplayName = userProfile.DisplayName;
            user.Email = userProfile.Email;
            user.Location = userProfile.Location;
            user.PreferredUserName = userProfile.DisplayName;
            user.RealName = userProfile.RealName;
            user.TwitterUserName = userProfile.TwitterUserName;

            if (!user.IsValid)
            {
                ModelState.AddModelErrors(user.RuleViolations);
                return View(userProfile);
            }

            _accountService.UpdateUser(user);

            return RedirectToAction("Display", new { id = userProfile.UserId, userName = userProfile.DisplayName });
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is FileNotFoundException)
            {
                filterContext.Result = View("NotFound");

                filterContext.ExceptionHandled = true;
            }
        }
    }
}
