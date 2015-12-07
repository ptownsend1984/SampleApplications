using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImageOrganizer.ViewModels;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading;
using ImageOrganizer.Presentation.Collections;
using System.Windows.Media;
using ImageOrganizer.Common.Utils;

namespace ImageOrganizer.ViewModels
{
    [Export]
    public class MainWindowViewModel : ViewModel
    {

        #region Global Variables

        private readonly DelegateCommand _BrowseFolderCommand;
        private readonly DelegateCommand _CloseFolderCommand;

        private readonly DelegateCommand _GotoFirstFileCommand;
        private readonly DelegateCommand _GotoPreviousFileCommand;
        private readonly DelegateCommand _GotoNextFileCommand;
        private readonly DelegateCommand _GotoLastFileCommand;
        private readonly DelegateCommand _GotoImageNameCommand;

        private readonly DelegateCommand _ZoomInCommand;
        private readonly DelegateCommand _ZoomOutCommand;
        private readonly DelegateCommand _ActualSizeCommand;
        private readonly DelegateCommand _ZoomFitCommand;
        private readonly DelegateCommand _FitWidthCommand;
        private readonly DelegateCommand _FitHeightCommand;

        private readonly DelegateCommand _RenameActiveImageCommand;
        private readonly DelegateCommand _DeleteActiveImageCommand;

        private readonly DelegateCommand _RefreshActiveImageCommand;

        private readonly DelegateCommand _AboutCommand;

        private readonly ActiveImagesViewModel _ActiveImagesViewModel;

        #endregion

        #region Properties

        public ICommand BrowseFolderCommand { get { return _BrowseFolderCommand; } }
        public ICommand CloseFolderCommand { get { return _CloseFolderCommand; } }
      
        public ICommand GotoFirstFileCommand { get { return _GotoFirstFileCommand; } }
        public ICommand GotoPreviousFileCommand { get { return _GotoPreviousFileCommand; } }
        public ICommand GotoNextFileCommand { get { return _GotoNextFileCommand; } }
        public ICommand GotoLastFileCommand { get { return _GotoLastFileCommand; } }
        public ICommand GotoImageNameCommand { get { return _GotoImageNameCommand; } }

        public ICommand ZoomInCommand { get { return _ZoomInCommand; } }
        public ICommand ZoomOutCommand { get { return _ZoomOutCommand; } }
        public ICommand ActualSizeCommand { get { return _ActualSizeCommand; } }
        public ICommand ZoomFitCommand { get { return _ZoomFitCommand; } }
        public ICommand FitWidthCommand { get { return _FitWidthCommand; } }
        public ICommand FitHeightCommand { get { return _FitHeightCommand; } }

        public ICommand RenameActiveImageCommand { get { return _RenameActiveImageCommand; } }
        public ICommand DeleteActiveImageCommand { get { return _DeleteActiveImageCommand; } }

        public ICommand RefreshActiveImageCommand { get { return _RefreshActiveImageCommand; } }

        public ICommand AboutCommand { get { return _AboutCommand; } }

        public ActiveImagesViewModel ActiveImagesViewModel { get { return _ActiveImagesViewModel; } }

        public ImageOrganizer.Windows.MainWindow Owner { get; internal set; }

        #endregion

        #region Events


        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            _BrowseFolderCommand = new DelegateCommand(DoBrowseFolderCommand, CanDoBrowseFolderCommand);
            _CloseFolderCommand = new DelegateCommand(DoCloseFolderCommand, CanDoCloseFolderCommand);

            _GotoFirstFileCommand = new DelegateCommand(DoGotoFirstFileCommand, CanDoGotoFirstFileCommand);
            _GotoPreviousFileCommand = new DelegateCommand(DoGotoPreviousFileCommand, CanDoGotoPreviousFileCommand);
            _GotoNextFileCommand = new DelegateCommand(DoGotoNextFileCommand, CanDoGotoNextFileCommand);
            _GotoLastFileCommand = new DelegateCommand(DoGotoLastFileCommand, CanDoGotoLastFileCommand);
            _GotoImageNameCommand = new DelegateCommand(DoGotoImageNameCommand, CanDoGotoImageNameCommand);

            _ZoomInCommand = new DelegateCommand(DoZoomInCommand, CanDoZoomInCommandCommand);
            _ZoomOutCommand = new DelegateCommand(DoZoomOutCommand, CanDoZoomOutCommandCommand);
            _ActualSizeCommand = new DelegateCommand(DoActualSizeCommand, CanDoActualSizeCommandCommand);
            _ZoomFitCommand = new DelegateCommand(DoZoomFitCommand, CanDoZoomFitCommandCommand);
            _FitWidthCommand = new DelegateCommand(DoFitWidthCommand, CanDoFitWidthCommandCommand);
            _FitHeightCommand = new DelegateCommand(DoFitHeightCommand, CanDoFitHeightCommandCommand);

            _RenameActiveImageCommand = new DelegateCommand(DoRenameActiveImageCommand, CanDoRenameActiveImageCommand);
            _DeleteActiveImageCommand = new DelegateCommand(DoDeleteActiveImageCommand, CanDoDeleteActiveImageCommand);

            _RefreshActiveImageCommand = new DelegateCommand(DoRefreshActiveImageCommand, CanDoRefreshActiveImageCommandCommand);

            _AboutCommand = new DelegateCommand(DoAboutCommand, CanDoAboutCommand);

            _ActiveImagesViewModel = new ActiveImagesViewModel();

            _ActiveImagesViewModel.ActiveFilesCollection.CollectionChanged += (s, e) =>
                {
                    _CloseFolderCommand.RaiseCanExecuteChanged();
                    RaisePositionCommandEvents();
                    RaiseZoomCommandEvents();
                    RaiseFileOperationEvents();
                };
            _ActiveImagesViewModel.ActiveImageChanged += (s, e) =>
                {
                    RaisePositionCommandEvents();
                };
            _ActiveImagesViewModel.PropertyChanged += (s, e) =>
                {
                    switch (e.PropertyName)
                    {
                        case "IsLoadingFiles":
                            RaiseFileOperationEvents();
                            break;
                    }
                };
        }

        #endregion

        #region Event Handlers


        #endregion

        #region Methods

        private void DoBrowseFolderCommand()
        {
            if (!CanDoBrowseFolderCommand())
                return;

            var Browser = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
            Browser.IsFolderPicker = true;
            var LastBrowsedDirectory = ImageOrganizer.Properties.Settings.Default.LastBrowsedDirectory;
            if (!string.IsNullOrEmpty(LastBrowsedDirectory) && System.IO.Directory.Exists(LastBrowsedDirectory))
                Browser.InitialDirectory = LastBrowsedDirectory;
            else
                Browser.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            var Result = Browser.ShowDialog();
            if (Result != Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
                return;

            ImageOrganizer.Properties.Settings.Default.LastBrowsedDirectory = Browser.FileName;
            this.ActiveImagesViewModel.LoadImages(Browser.FileName);
        }
        private bool CanDoBrowseFolderCommand()
        {
            return true;
        }

        private void DoCloseFolderCommand()
        {
            if (!CanDoCloseFolderCommand())
                return;

            this.ActiveImagesViewModel.CloseFolder();
        }
        private bool CanDoCloseFolderCommand()
        {
            return this.ActiveImagesViewModel.IsViewingFiles;
        }

        private void DoGotoFirstFileCommand()
        {
            if (!CanDoGotoFirstFileCommand())
                return;

            this.ActiveImagesViewModel.MoveToFirst();
        }
        private bool CanDoGotoFirstFileCommand()
        {
            return this.ActiveImagesViewModel.IsViewingFiles && !this.ActiveImagesViewModel.IsViewingFirstFile;
        }
        private void DoGotoPreviousFileCommand()
        {
            if (!CanDoGotoPreviousFileCommand())
                return;

            this.ActiveImagesViewModel.MoveToPrevious();
        }
        private bool CanDoGotoPreviousFileCommand()
        {
            return this.ActiveImagesViewModel.IsViewingFiles && !this.ActiveImagesViewModel.IsViewingFirstFile;
        }
        private void DoGotoNextFileCommand()
        {
            if (!CanDoGotoNextFileCommand())
                return;

            this.ActiveImagesViewModel.MoveToNext();
        }
        private bool CanDoGotoNextFileCommand()
        {
            return this.ActiveImagesViewModel.IsViewingFiles && !this.ActiveImagesViewModel.IsViewingLastFile;
        }
        private void DoGotoLastFileCommand()
        {
            if (!CanDoGotoLastFileCommand())
                return;

            this.ActiveImagesViewModel.MoveToLast();
        }
        private bool CanDoGotoLastFileCommand()
        {
            return this.ActiveImagesViewModel.IsViewingFiles && !this.ActiveImagesViewModel.IsViewingLastFile;
        }
        private void DoGotoImageNameCommand()
        {
            if (!CanDoGotoImageNameCommand())
                return;

            this.ActiveImagesViewModel.MoveToImageName();
        }
        private bool CanDoGotoImageNameCommand()
        {
            return this.ActiveImagesViewModel.IsViewingFiles;
        }

        private void DoZoomInCommand()
        {
            if (!CanDoZoomInCommandCommand())
                return;

            this.ActiveImagesViewModel.ZoomIn();
        }
        private bool CanDoZoomInCommandCommand()
        {
            return this.ActiveImagesViewModel.IsViewingFiles; 
        }
        private void DoZoomOutCommand()
        {
            if (!CanDoZoomOutCommandCommand())
                return;

            this.ActiveImagesViewModel.ZoomOut();
        }
        private bool CanDoZoomOutCommandCommand()
        {
            return this.ActiveImagesViewModel.IsViewingFiles; // && this.ActiveImagesViewModel.SelectedImage != null;
        }
        private void DoActualSizeCommand()
        {
            if (!CanDoActualSizeCommandCommand())
                return;

            this.ActiveImagesViewModel.ActualSize();
        }
        private bool CanDoActualSizeCommandCommand()
        {
            return this.ActiveImagesViewModel.IsViewingFiles; // && this.ActiveImagesViewModel.SelectedImage != null;
        }
        private void DoZoomFitCommand()
        {
            if (!CanDoZoomFitCommandCommand())
                return;

            this.ActiveImagesViewModel.ZoomFit();
        }
        private bool CanDoZoomFitCommandCommand()
        {
            return this.ActiveImagesViewModel.IsViewingFiles; // && this.ActiveImagesViewModel.SelectedImage != null;
        }
        private void DoFitWidthCommand()
        {
            if (!CanDoFitWidthCommandCommand())
                return;
            this.ActiveImagesViewModel.FitWidth();
        }
        private bool CanDoFitWidthCommandCommand()
        {
            return this.ActiveImagesViewModel.IsViewingFiles; // && this.ActiveImagesViewModel.SelectedImage != null;
        }
        private void DoFitHeightCommand()
        {
            if (!CanDoFitHeightCommandCommand())
                return;
            this.ActiveImagesViewModel.FitHeight();
        }
        private bool CanDoFitHeightCommandCommand()
        {
            return this.ActiveImagesViewModel.IsViewingFiles; // && this.ActiveImagesViewModel.SelectedImage != null;
        }

        private void DoRenameActiveImageCommand()
        {
            if (!CanDoRenameActiveImageCommand())
                return;

            this.ActiveImagesViewModel.RenameActiveImage();
        }
        private bool CanDoRenameActiveImageCommand()
        {
            return this.ActiveImagesViewModel.IsViewingFiles && !this.ActiveImagesViewModel.IsLoadingFiles;
        }
        private void DoDeleteActiveImageCommand()
        {
            if (!CanDoDeleteActiveImageCommand())
                return;

            this.ActiveImagesViewModel.DeleteActiveImage();
        }
        private bool CanDoDeleteActiveImageCommand()
        {
            return this.ActiveImagesViewModel.IsViewingFiles && !this.ActiveImagesViewModel.IsLoadingFiles;
        }

        private void DoRefreshActiveImageCommand()
        {
            if (!CanDoRefreshActiveImageCommandCommand())
                return;
            this.ActiveImagesViewModel.RefreshActiveImage();
        }
        private bool CanDoRefreshActiveImageCommandCommand()
        {
            return this.ActiveImagesViewModel.IsViewingFiles;
        }

        private void DoAboutCommand()
        {
            if (!CanDoAboutCommand())
                return;

            var AboutWindow = new ImageOrganizer.Windows.AboutWindow();
            AboutWindow.Owner = this.Owner;
            AboutWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            AboutWindow.ShowDialog();
        }
        private bool CanDoAboutCommand()
        {
            return true;
        }

        //private void Do()
        //{
        //    if (!CanDo))
        //        return;
        //}
        //private bool CanDo()
        //{
        //    return true;
        //}

        private void RaisePositionCommandEvents()
        {
            this._GotoFirstFileCommand.RaiseCanExecuteChanged();
            this._GotoPreviousFileCommand.RaiseCanExecuteChanged();
            this._GotoNextFileCommand.RaiseCanExecuteChanged();
            this._GotoLastFileCommand.RaiseCanExecuteChanged();
            this._GotoImageNameCommand.RaiseCanExecuteChanged();
        }
        private void RaiseZoomCommandEvents()
        {
            this._ZoomInCommand.RaiseCanExecuteChanged();
            this._ZoomOutCommand.RaiseCanExecuteChanged();
            this._ActualSizeCommand.RaiseCanExecuteChanged();
            this._ZoomFitCommand.RaiseCanExecuteChanged();
        }
        private void RaiseFileOperationEvents()
        {
            this._RenameActiveImageCommand.RaiseCanExecuteChanged();
            this._DeleteActiveImageCommand.RaiseCanExecuteChanged();
        }

        #endregion

    }
}