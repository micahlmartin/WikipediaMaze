using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.UI;
using System.IO;
using WikipediaMaze.Core;
using MvcContrib.Pagination;
using System.Web;
using System.Text.RegularExpressions;
using System.Linq;

namespace WikipediaMaze.Web
{
    public static class Extensions
    {

        #region Html

        public static string GenerateCaptcha(this HtmlHelper helper)
        {

            var captchaControl = new Recaptcha.RecaptchaControl
                    {
                        ID = "recaptcha",
                        Theme = "clean",
                        PublicKey = "6Lc-KwYAAAAAAOroQBwfMBz1rHzwi1ZlKtsmKPuj",
                        PrivateKey = "6Lc-KwYAAAAAANt8fE-8H3KtrFClNIz4f5FSqG4p"
                    };

            using (var htmlWriter = new HtmlTextWriter(new StringWriter(System.Globalization.CultureInfo.InvariantCulture)))
            {
                captchaControl.RenderControl(htmlWriter);

                return htmlWriter.InnerWriter.ToString();
            }

        }
        public static string Pager(this HtmlHelper htmlHelper, IPagination pagination, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            routeValues = routeValues ?? new RouteValueDictionary();
            var routeValueDictionary = new RouteValueDictionary(routeValues);
            if(!routeValueDictionary.ContainsKey("page"))
                routeValueDictionary.Add("page", 0);

            if(!routeValueDictionary.ContainsKey("controller"))
                routeValueDictionary.Add("controller", controllerName);
            
            int min = Math.Max(pagination.PageNumber - 2, 1);
            int max = Math.Min(pagination.PageNumber + 2, pagination.TotalPages);

            int startPage = Math.Min(min, 1);
            int endPage = Math.Max(max, pagination.TotalPages);
            int currentPage = pagination.PageNumber;

            var sb = new StringBuilder();
            var sw = new StringWriter(sb, System.Globalization.CultureInfo.InvariantCulture);
            var hw = new Html32TextWriter(sw, "    ");

            hw.WriteFullBeginTag("ul");

            if (pagination.HasPreviousPage)
            {
                hw.WriteLine();
                hw.WriteFullBeginTag("li");
                routeValueDictionary["page"] = currentPage - 1;
                hw.Write(htmlHelper.ActionLink("prev", actionName, routeValueDictionary,
                                               new Dictionary<string, object> {{"class", "previous"}}));
                hw.WriteEndTag("li");
            }

            if(min != startPage)
            {
                hw.WriteLine();
                hw.WriteFullBeginTag("li");
                routeValueDictionary["page"] = startPage;
                hw.Write(htmlHelper.ActionLink(startPage.ToInvariantString(), actionName, routeValueDictionary));
                hw.WriteEndTag("li");

                if (min - startPage > 1)
                {
                    hw.WriteFullBeginTag("li");
                    hw.Write("...");
                    hw.WriteEndTag("li");
                }
            }

            for (var i = min; i <= max; i++)
            {
                hw.WriteLine();
                hw.WriteFullBeginTag("li");
                routeValueDictionary["page"] = i;
                if (i == currentPage)
                {
                    hw.WriteFullBeginTag("span");
                    hw.Write(i.ToInvariantString());
                    hw.WriteEndTag("span");
                }
                else
                    hw.Write(htmlHelper.ActionLink(i.ToInvariantString(), actionName, routeValueDictionary));

                hw.WriteEndTag("li");
            }

            if(max != endPage)
            {
                if (endPage - max > 1)
                {
                    hw.WriteLine();
                    hw.WriteFullBeginTag("li");
                    hw.Write("...");
                    hw.WriteEndTag("li");
                }

                hw.WriteLine();
                hw.WriteFullBeginTag("li");
                routeValueDictionary["page"] = endPage;
                hw.Write(htmlHelper.ActionLink(endPage.ToInvariantString(), actionName, routeValueDictionary));
                hw.WriteEndTag("li");
            }

            if(pagination.HasNextPage)
            {
                hw.WriteLine();
                hw.WriteFullBeginTag("li");
                routeValueDictionary["page"] = currentPage + 1;
                hw.Write(htmlHelper.ActionLink("next", actionName, routeValueDictionary,
                                               new Dictionary<string, object> {{"class", "next"}}));
                hw.WriteEndTag("li");
            }

            hw.WriteLine();
            hw.WriteEndTag("ul");
            return sb.ToString();

            //StringBuilder sb1 = new StringBuilder();

            //int seed = pagination.PageNumber % pagination.PageSize == 0 ? pagination.PageNumber : pagination.PageNumber - (pagination.PageNumber % pagination.PageSize);

            //if (pagination.PageNumber > 1)
            //    sb1.AppendLine(htmlHelper.ActionLink("prev", "index", controllerName, new { page = pagination.PageNumber }));// String.Format("<a href=\"{0}/{1}\">Previous</a>", urlPrefix, pagination.PageNumber));

            //if (pagination.PageNumber - pagination.PageSize >= 1)
            //{
            //    var pageNumber = (pagination.PageNumber - pagination.PageSize) + 1)
            //    sb1.AppendLine(htmlHelper.ActionLink("...", "index", controllerName, new {page = pageNumber}));
            //}
            //for (int i = seed; i < Math.Round((pagination.TotalItems / 10) + 0.5) && i < seed + pagination.PageSize; i++)
            //{
            //    //sb1.AppendLine(htmlHelper.ActionLink("{0}".ToFormat(i + 1)) String.Format("<a href=\"{0}/{1}\">{1}</a>", urlPrefix, i + 1));
            //}

            //if (pagination.PageNumber + pagination.PageSize <= (Math.Round((pagination.TotalItems / 10) + 0.5) - 1))
            //    sb1.AppendLine(String.Format("<a href=\"{0}/{1}\">...</a>", urlPrefix, (pagination.PageNumber + pagination.PageSize) + 1));

            //if (currentPage < (Math.Round((pagination.TotalItems / 10) + 0.5) - 1))
            //    sb1.AppendLine(String.Format("<a href=\"{0}/{1}\">Next</a>", urlPrefix, pagination.PageNumber + 2));

            //return sb1.ToString();

            //var pagesToDisplay = 5;

            //var sb = new StringBuilder();
            //var textWriter = new StringWriter(sb);
            //var builder = new HtmlTextWriter(textWriter);

            ////opening ul tag
            //builder.RenderBeginTag("ul");

            ////prev tag
            //builder.RenderBeginTag("li");
            //if (pagination.HasPreviousPage)
            //    builder.Write(urlHelper.ActionLink("prev", actionName, new{id = pagination.PageNumber - 1}));
            //else
            //    builder.WriteEncodedUrl("prev");
            //builder.RenderEndTag();

            ////R
            //builder.RenderBeginTag("li");
            //if(pag)

            ////next tag
            //builder.RenderBeginTag("li");
            //if (pagination.HasPreviousPage)
            //    builder.Write(urlHelper.ActionLink("next", actionName, new { id = pagination.LastItem }));
            //else
            //    builder.WriteEncodedUrl("next");
            //builder.RenderEndTag();

            ////close ul tag
            //builder.RenderEndTag();
            //return sb.ToString();
        }

        #endregion

        #region ModelState

        public static void AddModelErrors(this ModelStateDictionary modelState, IEnumerable<RuleViolation> errors)
        {

            foreach (var issue in errors)
            {
                modelState.AddModelError(issue.PropertyName, issue.ErrorMessage);
            }
        }

        #endregion

    }

}
