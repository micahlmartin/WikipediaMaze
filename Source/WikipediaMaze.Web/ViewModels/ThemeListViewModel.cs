using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.Pagination;
using WikipediaMaze.Core;

namespace WikipediaMaze.Web.ViewModels
{
    public class ThemeListViewModel
    {
        public ThemeListViewModel(string pageTitle, IPagination<Theme> paginatedThemes, ThemeSortType sortType)
        {
            PageTitle = pageTitle;
            PaginatedThemes = paginatedThemes;
            SortType = sortType;
        }

        public string PageTitle { get; private set; }
        public IPagination<Theme> PaginatedThemes { get; private set; }
        public ThemeSortType SortType { get; private set; }

    }
}
