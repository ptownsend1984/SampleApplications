using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.ApplicationInsights.Extensibility;
using System.Configuration;

namespace GMailLabelCleanup.Core.Insights
{
    public class InsightsConfigInitializer : IContextInitializer
    {

        #region IContextInitializer Members

        public void Initialize(Microsoft.ApplicationInsights.DataContracts.TelemetryContext context)
        {
            context.Component.Version = typeof(TelemetryConfiguration).Assembly.GetName().Version.ToString();

            context.Properties.Add("EnvironmentMode", ConfigurationManager.AppSettings["EnvironmentMode"]);
        }

        #endregion

    }
}