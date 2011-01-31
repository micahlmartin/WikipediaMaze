using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using WikipediaMaze.Web.ViewModels;

namespace WikipediaMaze.Controllers
{
    [HandleErrorWithElmah]
    [SubControllerActionToViewData]
    public class PuzzlesController : Controller
    {
        #region Fields

        private readonly IPuzzleService _puzzleService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountService _accountService;
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ITwitterService _twitterService;

        #endregion

        #region Constructors

        public PuzzlesController(IPuzzleService puzzleService, IAuthenticationService authenticationService, IAccountService accountService, ITwitterService twitterService)
        {
            _puzzleService = puzzleService;
            _authenticationService = authenticationService;
            _accountService = accountService;
            _twitterService = twitterService;
        }

        #endregion

        /// <summary>
        /// Votes on a puzzle
        /// </summary>
        /// <remarks>
        /// If the puzzle has already been voted on in the specified direction, 
        /// then the vote is removed.
        /// </remarks>
        /// <param name="puzzleId">The Id of the puzzle to vote on</param>
        /// <param name="voteType">The vote type</param>
        /// <returns>
        /// The following JSON object is returned:
        /// 
        /// message: Contains an error message
        /// voteCount: The current vote count of the puzzle. Undefined if an error occurs.
        /// isUpVoted: True if the users vote is an up-vote otherwise false. Undefined if an error occurs. 
        /// </returns>
        public JsonResult VoteOnPuzzle(int puzzleId, VoteType voteType)
        {
            var result = _puzzleService.VoteOnPuzzle(puzzleId, voteType);

            //We need to format the vote count before sending it back.
            var voteCount = result.VoteCount.HasValue ? result.VoteCount.Value.FormatInteger() : null;

            return Json(new { result.ErrorMessage, VoteCount = voteCount, result.VoteType }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Themed(HeaderInfoController headerInfoController, SidebarController sidebarController, string themes, PuzzleSortType? sortType, int? page, int? pageSize)
        {
            sortType = sortType ?? PuzzleSortType.Newest;
            page = page ?? 1;
            pageSize = pageSize ?? 15;

            //Get sorted puzzles
            var themeList = Theme.GetThemesFromString(themes);
            IPagination<PuzzleDetailView> puzzles = _puzzleService.GetPuzzleDetailView(sortType.Value, page.Value, pageSize.Value, themeList);

            var viewModel = new PuzzleListViewModel(GetThemedPageTitle(sortType.Value, themeList), puzzles, sortType.Value, _authenticationService.IsAuthenticated, _authenticationService.CurrentUserId) {Themes = themeList};

            ViewData["themes"] = themes;
            return View(viewModel);
        }
        public ActionResult Index(HeaderInfoController headerInfoController, SidebarController sidebarController, PuzzleSortType? sortType, int? page, int? pageSize)
        {
            sortType = sortType ?? PuzzleSortType.Newest;
            page = page ?? 1;
            pageSize = pageSize ?? 15;

            //Get sorted puzzles
            IPagination<PuzzleDetailView> puzzles = _puzzleService.GetPuzzleDetailView(sortType.Value, page.Value, pageSize.Value);

            var viewModel = new PuzzleListViewModel(GetPageTitle(sortType.Value), puzzles, sortType.Value, _authenticationService.IsAuthenticated, _authenticationService.CurrentUserId);

            return View(viewModel);
        }

        public ActionResult Display(HeaderInfoController headerInfoController, SidebarController sidebarController, int id)
        {
            var puzzle = _puzzleService.GetPuzzleById(id);

            if (puzzle == null)
                return View("NotFound");

            //Only show puzzles that have not been verified to the user who created them.
            if (!puzzle.IsVerified)
            {
                if(puzzle.User.Id != _authenticationService.CurrentUserId)
                    return View("NotFound");
            }

            puzzle.Solutions = _puzzleService.GetSolutions(puzzle.Id);
            puzzle.Votes = _puzzleService.GetVotes(puzzle.Id);

            var latestSolution = puzzle.Solutions.Where(x => x.UserId == _authenticationService.CurrentUserId).LastOrDefault();
            if (latestSolution != null)
            {
                latestSolution.Steps = _puzzleService.GetSteps(latestSolution.Id);
            }

            var bestSolution = puzzle.Solutions.Where(x => x.UserId == _authenticationService.CurrentUserId).OrderByDescending(x => x.PointsAwarded).FirstOrDefault();
            if (bestSolution != null)
            {
                bestSolution.Steps = _puzzleService.GetSteps(bestSolution.Id);
            }

            var vote = puzzle.Votes.Where(x => x.UserId == _authenticationService.CurrentUserId).SingleOrDefault();
            var voteType = vote == null ? VoteType.None : vote.VoteType;
            var userSolutionCount = puzzle.Solutions.Where(x => x.UserId == _authenticationService.CurrentUserId).Count();
         
            var groupedSolutions = puzzle.Solutions.GroupBy(x => x.UserId);
            var topSolutions = new List<Solution>();
            foreach (var group in groupedSolutions)
	        {
                topSolutions.Add(group.OrderBy(x => x.StepCount).First());
	        }
            var puzzleLeaderBoard = topSolutions.OrderBy(x => x.StepCount).CreateOrderedEnumerable(x => x.DateCreated, new StandardComparer<DateTime>(), false).Take(10).Select(x => new SolutionViewModel(x, _accountService.GetUserById(x.UserId)));

            var viewModel = new PuzzleViewModel(puzzle, voteType, latestSolution, bestSolution, userSolutionCount, puzzleLeaderBoard, puzzle.User.Id == _authenticationService.CurrentUserId, false);

            return View(viewModel);
        }

        public ActionResult Themes(HeaderInfoController headerInfoController, SidebarController sidebarController, int? page, int? pagesize, ThemeSortType? sortType)
        {
            page = page ?? 1;
            pagesize = pagesize ?? 40;
            sortType = sortType ?? ThemeSortType.Popular;

            //Get sorted themes
            var themes = _puzzleService.GetThemes(pagesize.Value, page.Value, sortType.Value);
            var viewModel = new ThemeListViewModel(GetPageTitle(sortType.Value), themes, sortType.Value);
            return View(viewModel);

            ////Get sorted puzzles
            //IPagination<Puzzle> puzzles = _puzzleService.GetPuzzles(sortType.Value, page.Value, pageSize.Value);

            ////Get all the votes for the user for the puzzles that are to be displayed.
            //IEnumerable<Vote> votes = null;
            //IEnumerable<int> leadingPuzzles = null;
            //if (_authenticationService.IsAuthenticated)
            //{
            //    votes = _puzzleService.GetVotes(puzzles, _authenticationService.CurrentUserId);
            //    leadingPuzzles = _puzzleService.GetPuzzlesLedByUser(puzzles, _authenticationService.CurrentUserId);
            //}

            //var viewModel = new PuzzleListViewModel(GetPageTitle(sortType.Value), puzzles, sortType.Value, _authenticationService.IsAuthenticated, votes, leadingPuzzles);

            //return View(viewModel);
        }

        /// <summary>
        /// Displays the puzzles
        /// </summary>
        /// <param name="id">The id of the user who's profile is being displayed.</param>
        /// <param name="sortType">The id of the user who's profile is being displayed</param>
        /// <param name="page">The page number to show</param>
        /// <param name="pageSize">The size of the page to return.</param>
        public ActionResult PuzzleList(PuzzleSortType? sortType, int? page, int? pageSize)
        {
            sortType = sortType ?? PuzzleSortType.Newest;
            page = page ?? 1;
            pageSize = pageSize ?? 15;

            //Get sorted puzzles
            var puzzles = _puzzleService.GetPuzzleDetailView(sortType.Value, page.Value, pageSize.Value);

            var viewModel = new PuzzleListViewModel(GetPageTitle(sortType.Value), puzzles, sortType.Value, _authenticationService.IsAuthenticated, _authenticationService.CurrentUserId);

            return PartialView(viewModel);
        }
        
        public ActionResult TweetSolution(int id, int? puzzleId)
        {
            if (!_twitterService.IsAuthorized)
                _twitterService.RequestAuthorization();

            _twitterService.TweetSolution(id);

            return RedirectToAction("Display", new { id = puzzleId });
        }

        public ActionResult TweetPuzzle(int id)
        {
            if (!_twitterService.IsAuthorized)
                _twitterService.RequestAuthorization();

            _twitterService.TweetPuzzle(id);

            return RedirectToAction("Display", new { id = id });
        }

        //[OutputCache(Duration = 60)]
        public ActionResult GetThemesList()
        {
            var themes = _puzzleService.GetAllThemes();
            var sb = new StringBuilder();

            foreach (var item in themes)
            {
                sb.AppendLine(item.Name);
            }

            return Content(sb.ToString());
        }

        [AuthorizeRedirect]
        public ActionResult Create(HeaderInfoController headerInfoController, SidebarController sidebarController)
        {
            return View();
        }

        [CaptchaValidator]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeRedirect]
        public ActionResult Create(HeaderInfoController headerInfoController, SidebarController sidebarController, CreatePuzzleViewModel viewModel, bool captchaValid)
        {
            var themes = Theme.GetThemesFromString(viewModel.Themes);
         
            ValidatePuzzleForCreation(viewModel, themes, captchaValid);
            
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var result = _puzzleService.CreatePuzzle(viewModel.StartTopic, viewModel.EndTopic);
            
            if (!result.Success)
            {
                ModelState.AddModelErrors(result.RuleViolations);
                return View(viewModel);
            }

            _puzzleService.AddThemesToPuzzle(result.PuzzleId, _authenticationService.CurrentUserId, themes);

            return RedirectToAction("Display", new {id = result.PuzzleId});
        }
        private void ValidatePuzzleForCreation(CreatePuzzleViewModel vm, IEnumerable<string> themes, bool captchaValid)
        {
            if (!captchaValid)
                ModelState.AddModelError("Captcha", "Captcha is invalid");

            if (themes.Count() == 0)
                ModelState.AddModelError("Themes", "At least 1 theme is required");

            if (themes.Count() > 5)
                ModelState.AddModelError("Themes", "Can only select up to 5 themes");

            if((vm.StartTopic + "").Trim() == string.Empty)
                ModelState.AddModelError("StartTopic", "Start Topic cannot be blank");
            else if (!Uri.IsWellFormedUriString(vm.StartTopic, UriKind.Absolute))
                ModelState.AddModelError("StartTopic", "Start topic is not a valid Url");

            if ((vm.EndTopic + "").Trim() == string.Empty)
                ModelState.AddModelError("EndTopic", "End Topic cannot be blank");
            else if (!Uri.IsWellFormedUriString(vm.EndTopic, UriKind.Absolute))
                ModelState.AddModelError("EndTopic", "End topic is not a valid Url");

        }

        [AuthorizeRedirect]
        public ActionResult Delete(int id)
        {
            //Only allow logged in users with high enough reputation to edit the puzzle
            if (!_authenticationService.IsAuthenticated)
                return Json(false, JsonRequestBehavior.AllowGet);

            var puzzle = _puzzleService.GetPuzzleById(id);

            if (puzzle == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            if (puzzle.IsVerified || puzzle.User.Id != _authenticationService.CurrentUserId)
                return Json(false, JsonRequestBehavior.AllowGet);

            _puzzleService.DeletePuzzle(id);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRedirect]
        public ActionResult Edit(HeaderInfoController headerInfoController, SidebarController sidebarController, int id)
        {
            //Only allow logged in users with high enough reputation to edit the puzzle
            if (!_authenticationService.IsAuthenticated)
                return View("Unauthorized");

            if (_authenticationService.CurrentUser.Reputation < Settings.MinimumReputationToEditPuzzle)
                return View("NotPrivaledged");

            var puzzle = _puzzleService.GetPuzzleById(id);

            if (puzzle == null)
                return View("NotFound");

            //Only show puzzles that have not been verified to the user who created them.
            if (!puzzle.IsVerified)
            {
                if (puzzle.User.Id != _authenticationService.CurrentUserId)
                    return View("NotFound");
            }

            var vm = new EditPuzzleViewModel(puzzle);
            return View(vm);
        }

        [CaptchaValidator]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeRedirect]
        public ActionResult Edit(HeaderInfoController headerInfoController, SidebarController sidebarController, EditPuzzleViewModel viewModel, bool captchaValid)
        {
            if (_authenticationService.CurrentUser.Reputation < Settings.MinimumReputationToEditPuzzle)
                return View("NotPrivaledged");

            var themes = Theme.GetThemesFromString(viewModel.Themes);

            ValidatePuzzleEdit(viewModel, themes, captchaValid);

            if (!ModelState.IsValid)
                return View(viewModel);

            _puzzleService.AddThemesToPuzzle(viewModel.Id, _authenticationService.CurrentUserId, themes);

            return RedirectToAction("Display", new { id = viewModel.Id });
        }
        private void ValidatePuzzleEdit(EditPuzzleViewModel vm, IEnumerable<string> themes, bool captchaValid)
        {
            if (!captchaValid)
                ModelState.AddModelError("Captcha", "Captcha is invalid");

            if (themes.Count() == 0)
                ModelState.AddModelError("Themes", "At least 1 theme is required");

            if (themes.Count() > 5)
                ModelState.AddModelError("Themes", "Can only select up to 5 themes.");
        }

        public ActionResult Votes()
        {
            if (!_authenticationService.IsAuthenticated)
                return Json(null);

            var puzzleIdString = Request.Form["puzzleIds[]"];
            if (string.IsNullOrWhiteSpace(puzzleIdString))
                return Json(null);

            var puzzleIds = puzzleIdString.Split(new[]{','}, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x));

            var votes = _puzzleService.GetVotes(puzzleIds, _authenticationService.CurrentUserId);

            return Json(votes.Select(x => new { puzzleId = x.PuzzleId, voteType = x.VoteType }));
        }


        protected override void OnException(ExceptionContext filterContext)
        {
            _log.Error(filterContext.Exception.Message, filterContext.Exception);
        }

        private static string GetPageTitle(PuzzleSortType sortType)
        {
            switch (sortType)
            {
                case PuzzleSortType.Level:
                    return "Hardest Puzzles";
                case PuzzleSortType.Newest:
                    return "Newest Puzzles";
                case PuzzleSortType.Solutions:
                    return "Most Played Puzzles";
                case PuzzleSortType.Votes:
                    return "Highest Voted Puzzles";
                default:
                    throw new NotImplementedException("Need to handle sorting for type {0}".ToFormat(sortType));
            }
        }
        private static string GetPageTitle(ThemeSortType sortType)
        {
            switch (sortType)
            {
                case ThemeSortType.Alphabetical:
                    return "Themes Sorted Alphabetically";
                case ThemeSortType.Popular:
                    return "Most Popular Themes";
                default:
                    throw new NotFiniteNumberException("Need to handle sorting for type {0}".ToFormat(sortType));
            }
        }
        private static string GetThemedPageTitle(PuzzleSortType sortType, IEnumerable<string> themes)
        {
            var sb = new StringBuilder();
            switch (sortType)
            {
                case PuzzleSortType.Level:
                    sb.Append("Hardest");
                    break;
                case PuzzleSortType.Newest:
                    sb.Append("Newest");
                    break;
                case PuzzleSortType.Solutions:
                    sb.Append("Most Played");
                    break;
                case PuzzleSortType.Votes:
                    sb.Append("Highest Voted");
                    break;
                default:
                    throw new NotImplementedException("Need to handle sorting for type {0}".ToFormat(sortType));
            }
            sb.Append(" Puzzles Themed ");
            foreach (var theme in themes)
            {
                sb.AppendFormat("{0}, ", theme);
            }

            return sb.ToString().Trim(' ', ',');
        }
    }
}
