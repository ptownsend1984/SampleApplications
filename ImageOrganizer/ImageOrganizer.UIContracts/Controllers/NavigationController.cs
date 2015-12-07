using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImageOrganizer.Common.Contracts.Controllers;
using ImageOrganizer.Contracts.ViewModels;
using ImageOrganizer.Contracts.Windows;
using System.ComponentModel.Composition;

namespace ImageOrganizer.Contracts.Controllers
{
    [Export(typeof(INavigationController))]
    public class NavigationController : INavigationController
    {
        
        public System.IO.FileInfo GoToImageName(IEnumerable<System.IO.FileInfo> ActiveFiles)
        {
            if (ActiveFiles == null || ActiveFiles.Count() == 0)
                return null;

            using(var ViewModel = new GoToImageNameViewModel(ActiveFiles.ToArray()))
            {
                var Window = new GoToImageNameWindow();
                Window.Owner = System.Windows.Application.Current.MainWindow;
                Window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                Window.DataContext = ViewModel;
                var Result = Window.ShowDialog();
                if(Result.HasValue && Result.Value)
                {
                    return ViewModel.SelectedFileInfo;
                }
            }
            return null;
        }
    }
}