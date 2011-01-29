using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.Filters;
using WikipediaMaze.App;
using WikipediaMaze.Controllers.SubControllers;
using WikipediaMaze.Core;
using WikipediaMaze.Services;
using WikipediaMaze.Web;

namespace WikipediaMaze.Controllers
{
    public class CurrentPuzzleInfoViewModel
    {
        private readonly CurrentPuzzleInfo _puzzleInfo;
        private readonly bool _isBrowsing;

        public CurrentPuzzleInfoViewModel(CurrentPuzzleInfo puzzleInfo)
        {
            _puzzleInfo = puzzleInfo;
            _isBrowsing = puzzleInfo == null;
        }

        public int PuzzleId
        {
            get { return _puzzleInfo.PuzzleId; }
        }
        public int StepCount
        {
            get { return _puzzleInfo.Steps.Count + 1; }
        }
        public int PuzzleLevel
        {
            get { return _puzzleInfo.PuzzleLevel; }
        }
        public string StartTopic
        {
            get { return _puzzleInfo.StartTopic.Name.FormatTopic(); }
        }
        public string EndTopic
        {
            get { return _puzzleInfo.EndTopic.Name.FormatTopic(); }
        }
        public string EndTopicUrl
        {
            get
            {
                return _puzzleInfo.EndTopic.Name.FormatTopicAsUrl();
            }
        }
    }

    [HandleErrorWithElmah]
    [SubControllerActionToViewData]
    public class GameController : Controller
    {
        #region Fields

        private readonly IGameService _gameService;
        private readonly ITopicService _topicService;
        private readonly IAuthenticationService _authenticationService;

        #endregion

        #region Constructros
        public GameController(IGameService gameService, ITopicService topicService, IAuthenticationService authenticationService)
        {
            _gameService = gameService;
            _topicService = topicService;
            _authenticationService = authenticationService;
        }
        #endregion

        public ActionResult Continue(HeaderInfoController headerInfoController, string topic)
        {
            //For some reason the url /wiki?topic=Washington,_D.C is not being mapped properly
            //so I need to directly check the querystring.
            var newTopic = topic;
            if (string.IsNullOrEmpty(topic) && Request.QueryString.AllKeys.Contains("topic"))
            {
                newTopic = Request.QueryString["topic"];
            }

            if (!_gameService.IsPuzzleInProgress)
            {
                //Shows the main page if the url is the root www.wikipediamaze.com/wiki
                if (string.IsNullOrEmpty(newTopic))
                    newTopic = "Main_Page";

                var tp = _topicService.GetTopicByName(newTopic);
                if (tp != null)
                {
                    return Content(_topicService.GetTopicHtml(newTopic), "text/html");
                }
            }

            var puzzleInfo = _gameService.ContinuePuzzle(newTopic);

            if (puzzleInfo.IsSolved)
                return RedirectToAction("Display", "Puzzles", new {id = puzzleInfo.PuzzleId});

            return Content(_topicService.GetTopicHtml(puzzleInfo.CurrentTopic.Name), "text/html");
        }

        [AuthorizeRedirect]
        public ActionResult Start(HeaderInfoController headerInfoController, SidebarController sidebarController, int id)
        {
            var result = _gameService.StartPuzzle(id);

            return RedirectToAction("Continue", new { topic = result.StartTopic.Name });
        }

        [AuthorizeRedirect]
        public ActionResult GoBack(HeaderInfoController headerInfoController, SidebarController sidebarController)
        {
            var result = _gameService.GoBack();
            return RedirectToAction("Continue", new { topic = result.PreviousTopic.Name });
        }

        public JsonResult GetPuzzleInfo()
        {
            var puzzleInfo = _gameService.PuzzleInfo;

            if (puzzleInfo == null)
                return Json(new {IsBrowsing = true}, JsonRequestBehavior.AllowGet);

            var viewModel = new CurrentPuzzleInfoViewModel(puzzleInfo);
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is UnauthorizedAccessException ||
                filterContext.Exception is InvalidOperationException ||
                filterContext.Exception is FileNotFoundException)
            {
                filterContext.Result = View("GameError", new HandleErrorInfo(filterContext.Exception, "Game", "Error"));
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
