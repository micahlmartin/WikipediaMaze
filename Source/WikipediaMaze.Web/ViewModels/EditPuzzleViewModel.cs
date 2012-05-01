using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WikipediaMaze.Core;

namespace WikipediaMaze.Web.ViewModels
{
    public class EditPuzzleViewModel
    {
        #region Constructors
        public EditPuzzleViewModel() { }
        public EditPuzzleViewModel(Puzzle puzzle)
        {
            StartTopic = puzzle.StartTopic;
            EndTopic = puzzle.EndTopic;
            Id = puzzle.Id;

            var sb = new StringBuilder();
            foreach (var theme in puzzle.Themes)
            {
                sb.Append(theme + ", ");
            }
            var themeString = sb.ToString();
            if (themeString.Length > 0)
                themeString = themeString.TrimEnd(new char[] { ' ', ',' });

            Themes = themeString;
        }
        #endregion

        public string StartTopic { get; set; }
        public string EndTopic { get; set; }
        public string Themes { get; set; }
        public int Id { get; set; }
    }
}