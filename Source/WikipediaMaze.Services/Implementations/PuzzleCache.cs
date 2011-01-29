using System;
using System.Web;
using WikipediaMaze.Core;
using System.Web.Caching;

namespace WikipediaMaze.Services
{
    public class PuzzleCache : IPuzzleCache
    {
        #region Fields

        private IAuthenticationService _authenticationService;

        #endregion

        #region Constants

        private const string PUZZLE_INFO = "puzzleInfo/userId/";
        private const string TOPIC_INFO = "topic/";

        #endregion

        #region Constructors

        public PuzzleCache(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;   
        }

        #endregion

        #region IPuzzleCache Implementation

        public CurrentPuzzleInfo CurrentPuzzleInfo
        {
            get
            {
                return
                    HttpContext.Current.Session[PUZZLE_INFO] as
                    CurrentPuzzleInfo;
            }
        }

        public void SetCurrentPuzzleInfo(CurrentPuzzleInfo puzzleInfo)
        {
            if (puzzleInfo == null)
                ClearCurrentPuzzleInfo();
            else
                HttpContext.Current.Session.Add(PUZZLE_INFO, puzzleInfo); 
        }

        public void ClearCurrentPuzzleInfo()
        {
            HttpContext.Current.Session.Remove(PUZZLE_INFO);
        }

        #endregion
    }
}
