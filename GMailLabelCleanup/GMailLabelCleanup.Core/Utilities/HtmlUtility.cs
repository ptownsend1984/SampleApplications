using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using System.Web.WebPages;

namespace GMailLabelCleanup.Core.Utilities
{
    public static class HtmlUtility
    {

        /// <summary>
        /// Converts a viewbag object to Json
        /// </summary>
        /// <param name="viewBagObject">Object to make into Json</param>
        /// <returns>Javascript object</returns>
        public static IHtmlString ToJson(HtmlHelper html, dynamic viewBagObject)
        {
            var json = JsonConvert.SerializeObject(
                viewBagObject,
                Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
            );

            return html.Raw(json);
        }

        public static MvcHtmlString AddPartialScript(this HtmlHelper htmlHelper, int priority, Func<object, HelperResult> template)
        {
            if (htmlHelper == null)
                throw new ArgumentNullException("htmlHelper");
            if (template == null)
                throw new ArgumentNullException("template");

            htmlHelper.ViewContext.HttpContext.Items["_partialscript_" + Guid.NewGuid().ToString()] = new Tuple<int, Func<object, HelperResult>>(priority, template);
            return MvcHtmlString.Empty;
        }

        public static IHtmlString RenderPartialScripts(this HtmlHelper htmlHelper)
        {
            foreach (var tuple in GetPartialScripts(htmlHelper).OrderBy(o => o.Item1))
            {
                htmlHelper.ViewContext.Writer.Write(tuple.Item2(null));
            }
            return MvcHtmlString.Empty;
        }
        private static IEnumerable<Tuple<int, Func<object, HelperResult>>> GetPartialScripts(HtmlHelper htmlHelper)
        {
            return htmlHelper.ViewContext.HttpContext.Items.Keys
                .Cast<object>()
                .Where(o => o.ToString().StartsWith("_partialscript_"))
                .Select(o => htmlHelper.ViewContext.HttpContext.Items[o] as Tuple<int, Func<object, HelperResult>>)
                .Where(p => p != null);
        }

        public static MvcHtmlString Partial(this HtmlHelper helper, string partialViewName, object model, string prefix)
        {
            return helper.Partial(
                partialViewName,
                model,
                new ViewDataDictionary
                {
                    TemplateInfo = new TemplateInfo { HtmlFieldPrefix = prefix }
                });
        }

    }
}