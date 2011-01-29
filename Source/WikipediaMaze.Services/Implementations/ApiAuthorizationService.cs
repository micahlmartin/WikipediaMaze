using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Services.Interfaces;

namespace WikipediaMaze.Services.Implementations
{
    public class ApiAuthorizationService : IApiAuthorizationService
    {
        #region IApiAuthorizationService Members

        public bool IsAuthorized(string apiKey, string action)
        {
            return true;
        }

        #endregion
    }
}
