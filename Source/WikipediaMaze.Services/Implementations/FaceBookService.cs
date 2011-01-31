using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using facebook;
using WikipediaMaze.Services;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services
{
    public class FaceBookService : IFaceBookService
    {
        #region Fields

        private API _api;
        
        #endregion

        #region Constructors

        public FaceBookService()
        {
            _api = new API();
            _api.ApplicationKey = Settings.FacebookApiKey;
            _api.Secret = Settings.FacebookSecret;
        }

        #endregion

        public void Authenticate()
        {
            
        }
    }
}