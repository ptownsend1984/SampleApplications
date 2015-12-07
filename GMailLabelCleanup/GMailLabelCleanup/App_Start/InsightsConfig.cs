using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.ApplicationInsights.Extensibility;
using System.Configuration;
using GMailLabelCleanup.Core.Insights;

namespace GMailLabelCleanup
{
    public static class InsightsConfig
    {

        public static void Register()
        {
            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["Insights"];

            TelemetryConfiguration.Active.ContextInitializers.Add(new InsightsConfigInitializer());
        }

    }
}