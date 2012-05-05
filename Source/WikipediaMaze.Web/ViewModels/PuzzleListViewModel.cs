using System.Collections.Generic;
using System.Linq;
using MvcContrib.Pagination;
using WikipediaMaze.Core;
using WikipediaMaze.Services;

namespace WikipediaMaze.ViewModels
{
    public class PuzzleListViewModel
    {
        #region Fields

        private readonly string _pageTitle;

        #endregion

        #region Constructors

        public PuzzleListViewModel(string pageTitle, IPagination<Puzzle> paginatedPuzzles, PuzzleSortType sortType, bool isLoggedIn, int currentUserId)
        {
            PaginatedPuzzles = paginatedPuzzles;
            SortType = sortType;
            IsLoggedIn = isLoggedIn;
            _pageTitle = pageTitle;
            CurrentUserId = currentUserId;
        }

        #endregion
        public IPagination<Puzzle> PaginatedPuzzles { get; private set; }
        public PuzzleSortType SortType { get; private set; }
        public bool IsLoggedIn { get; private set; }
        public IEnumerable<string> Themes { get; set; }
        public int CurrentUserId { get; set; }

        public string PageTitle
        {
            get { return _pageTitle; }
        }

        public bool IsLeadingPuzzle(int puzzleId)
        {
            return PaginatedPuzzles.Any(x => x.LeaderId == CurrentUserId && x.Id == puzzleId);
        }
    }
}