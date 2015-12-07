using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace GMailLabelCleanup.Core.Attributes.ActionFilters
{
    public class InsightsErrorAttribute : HandleErrorAttribute
    {

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.Exception != null)
            {
                var telemetry = new ExceptionTelemetry(filterContext.Exception)
                {
                    Timestamp = DateTimeOffset.UtcNow                    
                };
                foreach (var item in GetErrorProperties(filterContext.Exception, 0))
                {
                    telemetry.Properties.Add(item.Item1, item.Item2);
                }

                new TelemetryClient().TrackException(telemetry);
            }

            base.OnException(filterContext);
        }
        private IEnumerable<Tuple<string, string>> GetErrorProperties(Exception ex, int level)
        {
            if (ex == null)
                yield break;

            yield return new Tuple<string, string>(string.Format("Message_Level{0}", level), ex.Message);
            yield return new Tuple<string, string>(string.Format("StackTrace_Level{0}", level), ex.StackTrace);

            foreach (var child in GetErrorProperties(ex.InnerException, level + 1))
            {
                yield return child;
            }
        }

    }
}