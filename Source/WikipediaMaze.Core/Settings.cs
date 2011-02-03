using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using System.Web;

namespace WikipediaMaze.Core
{
    public static class Settings
    {
        public static string FacebookApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["FacebookApiKey"];
            }
        }

        public static string FacebookSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["FacebookSecret"];
            }
        }

        public static string TwitterUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["TwitterUserName"];
            }
        }

        public static string TwitterPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["TwitterPassword"];
            }
        }

        public static bool IsOffline
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["IsOffline"]);
            }
        }

        public static int DefualtPageSize
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["DefualtPageSize"]);
            }
        }

        public static string SupportEmailAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["SupportEmailAddress"];
            }
        }

        public static int TopicExpirationDays
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["TopicExpirationDays"]);
            }
        }

        public static int SettingPuzzleInfoExpirationMinutes
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["SettingPuzzleInfoExpirationMinutes"]);
            }
        }

        public static string DefaultGravatarProvider
        {
            get
            {
                return ConfigurationManager.AppSettings["DefaultGravatarProvider"];
            }
        }

        public static string TwitterServiceUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["TwitterServiceUrl"];
            }
        }

        public static string TwitterConsumerKey
        {
            get
            {
                return ConfigurationManager.AppSettings["TwitterConsumerKey"];
            }
        }

        public static string TwitterConsumerSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["TwitterConsumerSecret"];
            }
        }

        public static int DownVoteReputationValue
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["DownVoteReputationValue"]);
            }
        }

        public static int MinimumReputationToUpVote
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["MinimumReputationToUpVote"]);
            }
        }

        public static int MinimumReputationToDownVote
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["MinimumReputationToDownVote"]);
            }
        }

        public static int UpVoteReputationValue
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["UpVoteReputationValue"]);
            }
        }

        public static int PointsAwardedToCreatorWhenPuzzleIsPlayed
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["PointsAwardedToCreatorWhenPuzzleIsPlayed"]);
            }
        }

        public static int AverageSolutionReputationValue
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["AverageSolutionReputationValue"]);
            }
        }

        public static int MinimumSolutionReputationValue
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["MinimumSolutionReputationValue"]);
            }
        }

        public static int MaximumDailyVoteLimit
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["MaximumDailyVoteLimit"]);
            }
        }

        public static int MinimumReputationToEditPuzzle
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["MinimumReputationToEditPuzzle"]);
            }
        }

        public static string AppName
        {
            get
            {
                return ((string)(ConfigurationManager.AppSettings["AppName"]));
            }
        }

        public static string WikipediaMazeConnection
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["WikipediaMazeConnection"].ConnectionString;
            }
        }

        public static int PuzzleLeaderUpdateInterval
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["PuzzleLeaderUpdateInterval"]);
            }
        }

        public static int ThemeCountUpdateInterval
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["ThemeCountUpdateInterval"]);
            }
        }

        public static int UpdatePuzzleSolutionCountInterval
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["UpdatePuzzleSolutionCountInterval"]);
            }
        }

        public static int AwardBadgeInterval
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["AwardBadgeInterval"]);
            }
        }

        public static int PuzzlesMaxPageSize
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["PuzzlesMaxPageSize"]);
            }
        }

        public static int FlairCacheMinutes
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["FlairCacheMinutes"]);
            }
        }

        public static string RpxApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["RpxApiKey"];
            }
        }

        public static string RpxDomain
        {
            get
            {
                return ConfigurationManager.AppSettings["RpxDomain"];
            }
        }

        public static string GoogleAnalytics
        {
            get
            {
                return ConfigurationManager.AppSettings["GoogleAnalytics"];
            }
        }

        public static string Host
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Request.Url.Port != 80)
                        return HttpContext.Current.Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);

                    return HttpContext.Current.Request.Url.GetComponents(UriComponents.Scheme, UriFormat.Unescaped) + "://" + HttpContext.Current.Request.Url.GetComponents(UriComponents.Host, UriFormat.Unescaped);
                }

                return string.Empty;
            }
        }

        public static string JSVersion
        {
            get
            {
#if DEBUG
                return DateTime.Now.Ticks.ToString();
#else
                return ConfigurationManager.AppSettings["JSVersion"];
#endif

            }
        }

        public static string CSSVersion
        {
            get
            {
#if DEBUG
                return DateTime.Now.Ticks.ToString();
#else
                return ConfigurationManager.AppSettings["CSSVersion"];
#endif

            }
        }

        public static string DisqusShortName
        {
            get
            {
                return ConfigurationManager.AppSettings["DisqusShortName"];
            }
        }

        public static string Domain
        {
            get
            {
                return ConfigurationManager.AppSettings["Domain"];
            }
        }
    }
}
