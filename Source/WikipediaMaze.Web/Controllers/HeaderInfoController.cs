using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib;
using WikipediaMaze.Core;
using WikipediaMaze.Services;

namespace WikipediaMaze.Controllers.SubControllers
{
    public class HeaderViewModel
    {
        #region Fields

        private readonly User _user;

        #endregion

        #region Constructors

        public HeaderViewModel(User user, bool isAuthenticated)
        {
            _user = user;
            IsAuthenticated = isAuthenticated;
        }

        #endregion

        public bool IsAuthenticated { get; private set; }

        public string DisplayName
        {
            get
            {
                return _user.DisplayName;
            }
        }
        public int UserId
        {
            get { return _user.Id; }
        }
        public int GoldBadgeCount
        {
            get { return _user.GoldBadgeCount; }
        }
        public int SilverBadgeCount
        {
            get { return _user.SilverBadgeCount; }
        }
        public int BronzeBadgeCount
        {
            get { return _user.BronzeBadgeCount; }
        }
        public int Reputation
        {
            get { return _user.Reputation; }
        }
        public int LeadingPuzzleCount
        {
            get { return _user.LeadingPuzzleCount; }
        }
        public string GetGravatarUrl(int size)
        {
            return _user.GetGravatarUrl(size);
        }
    }

    public class HeaderInfoController : SubController<HeaderViewModel>
    {
        #region Fields

        private readonly IAccountService _accountService;
        private readonly IAuthenticationService _authenticationService;

        #endregion

        #region Constructors

        public HeaderInfoController(IAccountService accountService, IAuthenticationService authenticationService)
        {
            _accountService = accountService;
            _authenticationService = authenticationService;
        }

        #endregion

        public ActionResult HeaderInfo()
        {
            var user = _authenticationService.CurrentUser;
            HeaderViewModel headerVM = null;

            if (user != null) 
                headerVM = new HeaderViewModel(user, _authenticationService.IsAuthenticated);
            
            return PartialView("HeaderInfo", headerVM);
        }
    }
}
