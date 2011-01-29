using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services
{
    public interface IAuthenticationService
    {
        int CurrentUserId { get; }
        User CurrentUser { get; }
        bool IsAuthenticated { get; }
        void SignIn(int userId, bool rememberUser);
        void SignOut();
    }
}
