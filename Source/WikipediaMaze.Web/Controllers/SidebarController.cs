using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcContrib;
using MvcContrib.Pagination;
using WikipediaMaze.Core;
using WikipediaMaze.Services;

namespace WikipediaMaze.Controllers.SubControllers
{
    public class SidebarViewModel
    {
        #region Fields

        #endregion

        #region Constructors

        public SidebarViewModel(IEnumerable<Theme> topThemes, IEnumerable<User> topUsers)
        {
            TopThemes = topThemes;
            TopUsers = topUsers;
        }

        #endregion

        public IEnumerable<Theme> TopThemes { get; private set; }

        public IEnumerable<User> TopUsers { get; private set; }

    }

    public class SidebarController : SubController
    {

        #region Fields

        private readonly IPuzzleService _puzzleService;
        private readonly IAccountService _accountService;

        #endregion

        #region Constructors

        public SidebarController(IPuzzleService puzzleService, IAccountService accountService)
        {
            _puzzleService = puzzleService;
            _accountService = accountService;
        }
            
        #endregion

        public ActionResult Sidebar()
        {
            var topUsers = _accountService.GetLeaderBoard(1, 5);
            var topThemes = _puzzleService.GetThemes(20, 1, ThemeSortType.Popular);

            var sidebarVM = new SidebarViewModel(topThemes, topUsers);

            return PartialView("Sidebar", sidebarVM);
        }

    }
}
