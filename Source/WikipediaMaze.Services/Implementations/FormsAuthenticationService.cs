using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WikipediaMaze.Data;
using WikipediaMaze.Core;
using WikipediaMaze.Data.Mongo;

namespace WikipediaMaze.Services
{
    public class FormsAuthenticationService : IAuthenticationService
    {
        #region Fields

        private readonly IRepository _repository;

        #endregion

        #region Constructors

        public FormsAuthenticationService(MongoRepository repository)
        {
            _repository = repository;
        }

        #endregion

        public int CurrentUserId
        {
            get
            {
                var userId = 0;
                int.TryParse(HttpContext.Current.User.Identity.Name, out userId);
                return userId;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        public void SignIn(int userId, bool rememberUser)
        {
            FormsAuthentication.SetAuthCookie(userId.ToInvariantString(), rememberUser);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        #region IAuthenticationService Members

        public User CurrentUser
        {
            get { return _repository.All<User>().ById(CurrentUserId); }
        }

        #endregion
    }
}
