using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace WikipediaMaze.Core
{
    public class Theme
    {
        public virtual string Name { get; set; }
        public virtual int UserId { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual int Count { get; set; }

        public static IEnumerable<string> GetThemesFromString(string themeString)
        {
            themeString += "";
            var themes = new List<string>();
            foreach (var theme in themeString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var formattedTheme = theme;
                formattedTheme = FormatTheme(formattedTheme);

                if (!string.IsNullOrEmpty(formattedTheme))
                    themes.Add(formattedTheme);
            }

            return themes;
        }

        private static string FormatTheme(string theme)
        {
            theme += "";
            var formattedTheme = IllegalCharacters.Replace(theme, "");
            formattedTheme = WhiteSpaceCharacters.Replace(formattedTheme.Trim(), "-");
            formattedTheme = WhiteSpaceReplacement.Replace(formattedTheme, "-");

            return formattedTheme;
        }

        private static Regex IllegalCharacters = new Regex("@[^\\w\\s]");
        private static Regex WhiteSpaceCharacters = new Regex("\\s+");
        private static Regex WhiteSpaceReplacement = new Regex("-+");
    }
}