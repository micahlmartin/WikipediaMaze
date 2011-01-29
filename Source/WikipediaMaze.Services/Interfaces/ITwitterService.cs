using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikipediaMaze.Services
{
    public interface ITwitterService
    {
        void RequestAuthorization();
        bool IsAuthorized { get; }
        //void UpdateStatus(string status);

        void TweetSolution(int id);

        void TweetPuzzle(int id);
    }
}
