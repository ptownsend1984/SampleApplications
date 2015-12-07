using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImageOrganizer.ViewModels;
using System.IO;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using ImageOrganizer.ViewModels.DataAnnotations;
using Microsoft.Practices.Prism.Commands;
using ImageOrganizer.Presentation.Collections;
using ImageOrganizer.Common.Utils;

namespace ImageOrganizer.Contracts.ViewModels
{
    public class GoToImageNameViewModel : DialogWindowViewModel
    {

        #region Global Variables

        private IEnumerable<FileInfo> ActiveFiles;
        private readonly ObservableCollection<FileInfo> _AutoCompleteList;
        private readonly ListCollectionView _AutoCompleteListView;

        private string _FileName;
        private bool _FileExists;
        private FileInfo _SelectedFileInfo;

        #endregion

        #region Properties

        public IEnumerable<FileInfo> AutoCompleteList { get { return _AutoCompleteList; } }
        public ICollectionView AutoCompleteListView { get { return _AutoCompleteListView; } }
        private CancellationTokenSource CancellationTokenSource { get; set; }

        [FileNameCharactersAttribute(ErrorMessage = "This file name contains invalid characters.")]
        [TrueInvalid("FileExists", ErrorMessage = "This file does not exist.")]
        public string FileName
        {
            get { return _FileName; }
            set
            {
                if (_FileName == value) return;
                _FileName = value;
                this.RaisePropertyChanged(() => this.FileName);
                this.RaiseOKCommandCanExecuteChanged();
            }
        }
        public bool FileExists
        {
            get { return _FileExists; }
            set
            {
                if (_FileExists == value) return;
                _FileExists = value;
                this.RaisePropertyChanged(() => this.FileExists);
                this.RaisePropertyChanged(() => this.FileName);
            }
        }
        public FileInfo SelectedFileInfo
        {
            get { return _SelectedFileInfo; }
            set
            {
                if (_SelectedFileInfo == value) return;
                _SelectedFileInfo = value;
                RaisePropertyChanged(() => this.SelectedFileInfo);
            }
        }

        #endregion

        #region Events


        #endregion

        #region Constructor

        public GoToImageNameViewModel(IEnumerable<FileInfo> ActiveFiles)
        {
            if (ActiveFiles == null)
                throw new ArgumentNullException("ActiveFiles");
            if (ActiveFiles.Count() == 0)
                throw new ArgumentException("ActiveFiles cannot be empty");
            this.ActiveFiles = ActiveFiles;

            _AutoCompleteList = new SyncObservableCollection<FileInfo>();
            _AutoCompleteListView = new SyncListCollectionView(_AutoCompleteList);
            _AutoCompleteListView.CustomSort = new NaturalStringComparer();

            LoadAutoCompleteAsync();
        }

        #endregion

        #region Event Handlers


        #endregion

        #region Methods

        private void LoadAutoCompleteAsync()
        {
            this.CancellationTokenSource = new System.Threading.CancellationTokenSource();
            System.Threading.Tasks.Task.Factory.StartNew(
                (o) => { LoadAutoCompleteCore(this.CancellationTokenSource.Token); }, null, this.CancellationTokenSource.Token
                );
        }
        private void LoadAutoCompleteCore(CancellationToken Token)
        {
            var CancellationTokenSource = this.CancellationTokenSource;
            try
            {
                this.CancellationTokenSource.Token.ThrowIfCancellationRequested();
                foreach (var Item in this.ActiveFiles)
                {
                    this.CancellationTokenSource.Token.ThrowIfCancellationRequested();
                    if (!ImageFileUtils.IsSkippedExtension(Item.Extension))
                    {
                        this._AutoCompleteList.Add(Item);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
            }
        }

        protected override void DoOKCommand()
        {
            this.FileExists = false;
            if (!this.IsValid) return;

            var FileName = this.FileName;

            FileInfo SelectedFileInfo = null;            

            //Look for exact match
            SelectedFileInfo = (from f in this.ActiveFiles
                                where f.Name.Equals(FileName, StringComparison.OrdinalIgnoreCase)
                                select f).FirstOrDefault();

            //If no match, wildcard off the end
            if (SelectedFileInfo == null)
            {
                //Look for exact match
                SelectedFileInfo = (from f in this.ActiveFiles
                                    where f.Name.StartsWith(FileName, StringComparison.OrdinalIgnoreCase)
                                    select f).FirstOrDefault();
            }
            if (SelectedFileInfo != null)
            {
                this.SelectedFileInfo = SelectedFileInfo;
                this.RaiseRequestClose(true);
            }
            else
            {
                this.FileExists = true;
            }

            return;
        }
        protected override bool CanDoOkCommand()
        {
            return !string.IsNullOrEmpty(this.FileName);
        }

        protected override string GetWindowTitle()
        {
            return "Go To Image";
        }
        protected override void RaiseRequestClose(bool? DialogResult)
        {
            if (this.CancellationTokenSource.Token.CanBeCanceled)
                this.CancellationTokenSource.Cancel();

            base.RaiseRequestClose(DialogResult);
        }

        protected override void OnDispose()
        {
            try
            {
                this.CancellationTokenSource.Dispose();
            }
            catch { }
            this.CancellationTokenSource = null;
            this.ActiveFiles = null;
            base.OnDispose();
        }

        #endregion

    }
}