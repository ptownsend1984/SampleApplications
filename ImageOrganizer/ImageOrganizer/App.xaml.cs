using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ImageOrganizer.Common.Utils;

namespace ImageOrganizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //http://cs.rthand.com/blogs/blog_with_righthand/archive/2005/12/09/246.aspx
            if (ImageOrganizer.Properties.Settings.Default.NeedsUpgrade)
            {
                ImageOrganizer.Properties.Settings.Default.Upgrade();
                ImageOrganizer.Properties.Settings.Default.NeedsUpgrade = false;
                ImageOrganizer.Properties.Settings.Default.Save();
            }

            this.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;

            var Bootstrapper = new Bootstrapper();
            Bootstrapper.Run();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ImageOrganizer.Properties.Settings.Default.Save();

            base.OnExit(e);
        }
    }
}
