using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Web;
using MvcContrib.Pagination;

namespace WikipediaMaze.Core
{
    public static class Extensions
    {
        #region IO

        public static byte[] Decompress(this byte[] data)
        {

            using (var ms = new MemoryStream(data))
            {
                using (var s = new GZipStream(ms, CompressionMode.Decompress))
                {
                    using (var output = new MemoryStream())
                    {
                        var buffer = new byte[8192];
                        var read = 0;
                        while ((read = s.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            output.Write(buffer, 0, read);
                        }

                        return output.ToArray();
                    }
                }
            }
        }
        public static byte[] Decompress(this Stream data)
        {
            using (var s = new GZipStream(data, CompressionMode.Decompress))
            {
                using (var output = new MemoryStream())
                {
                    var buffer = new byte[8192];
                    var read = 0;
                    while ((read = s.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        output.Write(buffer, 0, read);
                    }

                    return output.ToArray();
                }
            }
        }
        public static byte[] ReadToEnd(this Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                var buffer = new byte[8192];
                int read = 0;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public static string EncodeAsFileName(this string fileName)
        {
            return Regex.Replace(fileName, "[" + Regex.Escape(new string(Path.GetInvalidFileNameChars()) + "\"") + "]", " ");
        }

        #endregion

        public static IPagination<T> AsCustomPagination<T>(this IEnumerable<T> collection, int page, int pageSize, int totalItems)
        {
            return new CustomPagination<T>(collection, page, pageSize, totalItems);
        }

        /// <summary>
        /// Return an MD5 hash of the string.
        /// </summary>
        public static string ToMD5Hash(this string value)
        {
            value += ""; //Make sure string is not null

            var encoder = new UTF8Encoding();
            var md5Hasher = new MD5CryptoServiceProvider();

            byte[] hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(value));

            var sb = new StringBuilder(hashedBytes.Length * 2);
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                sb.Append(hashedBytes[i].ToString("X2", System.Globalization.CultureInfo.InvariantCulture));
            }

            // And return it
            return sb.ToString();
        }

        public static string AsGravatarUrl(this string email, int? size)
        {
            email += "";

            var hash = email.ToLowerInvariant().Trim().ToMD5Hash().ToLowerInvariant();
            var url = "http://www.gravatar.com/avatar.php?" + "gravatar_id=" + hash + "&amp;d=identicon";

            if (size.HasValue)
            {
                url += "&amp;size={0}".ToFormat(size.Value.ToString(System.Globalization.CultureInfo.CurrentCulture));
            }

            return url;
        }

        /// <summary>
        /// Replaces the format item in a specified System.String with the text equivalent of the value of a corresponding System.Object instance in a specified array.
        /// </summary>
        public static string ToFormat(this string format, params object[] args)
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, format, args);
        }

        public static string ToInvariantString(this int value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static T Pop<T>(this IList<T> items, int index)
        {
            var item = items.ElementAt(index);
            items.RemoveAt(index);
            return item;
        }

        public static string FormatTopic(this string topic)
        {
            var topicName = topic;

            if (Uri.IsWellFormedUriString(topicName, UriKind.Absolute))
                topicName = RegularExpressions.WikipediaLinkRegex.Match(topic).Groups["Topic"].Value;

            topicName = Uri.UnescapeDataString(topicName);

            return topicName.Replace("_", " ");
        }
        public static string FormatTopicAsUrl(this string topic)
        {
            return "http://www.wikipedia.org/wiki/{0}".ToFormat(topic);
        }
        public static string FormatInteger(this int value)
        {
            return value.ToString("N0");
            //var millions = value / 1000000;
            //if (millions >= 1)
            //{
            //    return millions.ToString(System.Globalization.CultureInfo.CurrentCulture) + "m";
            //}

            //var thousands = value / 1000;
            //if (thousands >= 10)
            //{
            //    return thousands.ToString(System.Globalization.CultureInfo.CurrentCulture) + "k";
            //}

            //return value.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
        }
        public static string FormatDate(this DateTime value)
        {
            var timeDiff = DateTime.Now - value;

            //Check years
            var years = (timeDiff.Days / 365);
            if (years == 1)
                return "1 year ago";
            if (years > 1)
                return "{0} years ago".ToFormat(years.ToString(System.Globalization.CultureInfo.CurrentCulture));

            //Check months
            var months = timeDiff.Days / 30;
            if (months == 1)
                return "1 month ago";
            if (months > 1)
                return "{0} months ago".ToFormat(months.ToString(System.Globalization.CultureInfo.CurrentCulture));

            //Check days
            var days = (int)timeDiff.TotalDays;
            if (days == 1)
                return "1 day ago";
            if (days > 1)
                return "{0} days ago".ToFormat(days.ToString(System.Globalization.CultureInfo.CurrentCulture));

            var hours = (int)timeDiff.TotalHours;
            if (hours == 1)
                return "1 hour ago";
            if (hours > 1)
                return "{0} hours ago".ToFormat(hours.ToString(System.Globalization.CultureInfo.CurrentCulture));

            //Check minutes
            var minutes = (int)timeDiff.TotalMinutes;
            if (minutes == 1)
                return "1 min ago";
            if (minutes > 1)
                return "{0} mins ago".ToFormat(minutes.ToString(System.Globalization.CultureInfo.CurrentCulture));

            //Check Seconds
            var seconds = (int)timeDiff.TotalSeconds;
            if (seconds > 1)
                return "{0} secs ago".ToFormat(seconds.ToString(System.Globalization.CultureInfo.CurrentCulture));

            return "1 sec ago";
        }
        public static int FormatAge(this DateTime birthDate)
        {
            // get the difference in years
            var years = DateTime.Now.Year - birthDate.Year;
            // subtract another year if we're before the
            // birth day in the current year
            if (DateTime.Now.Month < birthDate.Month ||
                (DateTime.Now.Month == birthDate.Month &&
                DateTime.Now.Day < birthDate.Day))
                years--;

            return years;
        }
        public static void AddRange<T>(this IList<T> collection, IEnumerable<T> itemsToAdd)
        {
            foreach (var item in itemsToAdd)
            {
                collection.Add(item);
            }
        }
        public static string HtmlEncode(this string value)
        {
            return HttpUtility.HtmlEncode(value);
        }
        public static string UrlEncode(this string value)
        {
            return HttpUtility.UrlEncode(value);
        }
        public static string FormatTitleForUrl(this string value)
        {
            value += "";
            value = value.Replace(' ', '-');
            value = value.Replace('/', '-');

            return value;
        }
        public static bool EqualsOrdinalIgnoreCase(this string obj, string value)
        {
            return obj.Equals(value, StringComparison.OrdinalIgnoreCase);
        }
    }
}
