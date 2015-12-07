using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImageOrganizer.Common.Contracts.Controllers;
using System.ComponentModel.Composition;
using ImageOrganizer.Contracts.Windows;
using ImageOrganizer.Contracts.ViewModels;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using ImageOrganizer.Common.Utils;

namespace ImageOrganizer.Contracts.Controllers
{
    [Export(typeof(IFileOperationsController))]
    public class FileOperationsController : IFileOperationsController
    {

        #region Methods

        public void RenameFile(Common.Contracts.Images.IImageViewModel ImageViewModel)
        {
            if (ImageViewModel == null)
                throw new ArgumentNullException("ImageViewModel");

            using (var ViewModel = new RenameFileViewModel(ImageViewModel.FileInfo))
            {
                var Window = new RenameFileWindow();
                Window.DataContext = ViewModel;
                Window.Owner = System.Windows.Application.Current.MainWindow;
                Window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                var Result = Window.ShowDialog();
                if (Result.HasValue && Result.Value)
                {
                    ImageViewModel.NotifyAllPropertiesChanged();
                }
            }
        }
        public bool DeleteFile(FileInfo FileInfo)
        {
            if (FileInfo == null)
                return false;
            var Message =  "Are you sure you want to delete " + FileInfo.Name + "?";
            var Title = "Delete File";
            if (System.Windows.MessageBox.Show(
                System.Windows.Application.Current.MainWindow, 
                Message, Title, 
                System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question
                ) != System.Windows.MessageBoxResult.Yes)
                return false;

            if (WinAPIUtils.RecycleFile(FileInfo.FullName))
                return true;
            else
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "Could not delete " + FileInfo.Name, "Delete File", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
        }

        #endregion

    }
}