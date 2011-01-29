using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using facebook;
using WikipediaMaze.Services;
using WikipediaMaze.Core.Properties;

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
            _api.ApplicationKey = Settings.Default.FacebookApiKey;
            _api.Secret = Settings.Default.FacebookSecret;
        }

        #endregion

        public void Authenticate()
        {
            
        }
    }
}