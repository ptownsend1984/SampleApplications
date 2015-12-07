using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImageOrganizer.ViewModels;
using System.IO;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel.DataAnnotations;
using ImageOrganizer.ViewModels.DataAnnotations;
using System.Windows.Input;
using System.ComponentModel.Composition;
using ImageOrganizer.Common.Extensions;
using System.Windows.Data;
using System.ComponentModel;
using ImageOrganizer.Presentation.Collections;
using ImageOrganizer.Common.Utils;
using System.Threading;

namespace ImageOrganizer.Contracts.ViewModels
{
    public class RenameFileViewModel : DialogWindowViewModel
    {

        #region Static Members

        //TODO: Make setting out of AllowExtensionNameConflicts
        private static bool _AllowExtensionNameConflicts;

        #endregion

        #region Global Variables

        private FileInfo _ExistingFileInfo;

        private string _NewFileName;
        private bool _IsNameConflict;

        private readonly ObservableCollection<string> _AutoCompleteList;
        private readonly ListCollectionView _AutoCompleteListView;
        private readonly ListCollectionView _NearbyMatchesListView;

        #endregion

        #region Properties

        public FileInfo ExistingFileInfo { get { return _ExistingFileInfo; } }

        public IEnumerable<string> AutoCompleteList { get { return _AutoCompleteList; } }
        public ICollectionView AutoCompleteListView { get { return _AutoCompleteListView; } }
        public ICollectionView NearbyMatchesListView { get { return _NearbyMatchesListView; } }
        private CancellationTokenSource CancellationTokenSource { get; set; }

        //TODO: Oddity with Validator - does not pick up multiple of the same attribute type
        [Required(ErrorMessage = "New file name cannot be blank.")]
        [TrueInvalid("IsNameConflict", ErrorMessage = "This file name is already in use.")]
        [FalseInvalid("IsValidLength", ErrorMessage = "This file name is too long.")]
        [FileNameCharactersAttribute(ErrorMessage = "This file name contains invalid characters.")]
        public string NewFileName
        {
            get { return _NewFileName; }
            set
            {
                if (_NewFileName == value) return;
                _NewFileName = value;
                this.RaisePropertyChanged(() => this.NewFileName);
                this.AutoCompleteListView.Refresh();
                SetNearbyMatch();
            }
        }

        public bool IsValidLength
        {
            get
            {
                if (string.IsNullOrEmpty(this.NewFileName))
                    return false;
                var FileNameWithExt = this.NewFileName + this.ExistingFileInfo.Extension;
                return !ImageFileUtils.IsLongFileName(System.IO.Path.Combine(this.ExistingFileInfo.DirectoryName, FileNameWithExt));
            }
        }
        public bool IsNameConflict
        {
            get { return _IsNameConflict; }
            set
            {
                if (_IsNameConflict == value) return;
                _IsNameConflict = value;
                this.RaisePropertyChanged(() => this.NewFileName);
                this.RaisePropertyChanged(() => this.IsNameConflict);
            }
        }

        public bool AllowExtensionNameConflicts
        {
            get { return _AllowExtensionNameConflicts; }
            set
            {
                if (_AllowExtensionNameConflicts == value) return;
                _AllowExtensionNameConflicts = value;
                RaisePropertyChanged(() => this.AllowExtensionNameConflicts);
            }
        }

        #endregion

        #region Events


        #endregion

        #region Constructor

        public RenameFileViewModel(FileInfo ExistingFileInfo)
        {
            if (ExistingFileInfo == null)
                throw new ArgumentNullException("ExistingFileInfo");

            this._ExistingFileInfo = ExistingFileInfo;
            _NewFileName = System.IO.Path.GetFileNameWithoutExtension(this.ExistingFileInfo.Name);

            _AutoCompleteList = new SyncObservableCollection<string>();
            _AutoCompleteListView = new SyncListCollectionView(_AutoCompleteList);
            _AutoCompleteListView.CustomSort = new NaturalStringComparer();
            _AutoCompleteListView.Filter = AutoCompleteListView_Filter;

            _NearbyMatchesListView = new SyncListCollectionView(_AutoCompleteList);
            _NearbyMatchesListView.CustomSort = new NaturalStringComparer();

            LoadAutoCompleteAsync();
        }

        #endregion

        #region Event Handlers

        private bool AutoCompleteListView_Filter(object Item)
        {
            if (Item == null)
                return false;
            if (string.IsNullOrEmpty(this.NewFileName))
                return true;
            var Value = Item.ToString();
            return Value.IndexOf(this.NewFileName, StringComparison.CurrentCultureIgnoreCase) > -1;
        }

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
                foreach (var Item in this.ExistingFileInfo.Directory.EnumerateFiles())
                {
                    this.CancellationTokenSource.Token.ThrowIfCancellationRequested();
                    if (!ImageFileUtils.IsSkippedExtension(Item.Extension))
                    {
                        this._AutoCompleteList.Add(System.IO.Path.GetFileNameWithoutExtension(Item.Name));
                    }
                }
                this.SetNearbyMatch();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
            }
        }

        private void SetNearbyMatch()
        {
            int MatchIndex;
            var Value = this.NewFileName;

            //Use existing file name if new file name is empty
            if (string.IsNullOrEmpty(Value))
                Value = System.IO.Path.GetFileNameWithoutExtension(this.ExistingFileInfo.Name);

            MatchIndex = FindNearbyMatchIndex(Value);
            if (MatchIndex < 0)
                return;
            this.NearbyMatchesListView.MoveCurrentToPosition(MatchIndex);
        }
        private int FindNearbyMatchIndex(string Value)
        {
            if (this.NearbyMatchesListView.IsEmpty)
                return -1;
            if (string.IsNullOrEmpty(Value))
                return 0;
            int Index = -1;
            while (Index == -1 && !string.IsNullOrEmpty(Value))
            {
                var StartsWithItem = this.NearbyMatchesListView.Cast<string>().Where((o) => o.StartsWith(Value, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (StartsWithItem != null)
                {
                    Index = this._NearbyMatchesListView.IndexOf(StartsWithItem);
                }
                if (Index == -1 && !string.IsNullOrEmpty(Value))
                    Value = Value.Substring(0, Value.Length - 1);
            }
            return Index;
        }

        protected override void DoOKCommand()
        {
            string NewFileNameWithExt;
            if (!string.IsNullOrEmpty(this.NewFileName))
                NewFileNameWithExt = this.NewFileName + this.ExistingFileInfo.Extension;
            else
                NewFileNameWithExt = null;

            try
            {
                //Validate NewFileName's dependent properties
                if (this.NewFileName.IsValidFileName() && this.IsValidLength)
                {
                    if (this.AllowExtensionNameConflicts)
                    {
                        this.IsNameConflict = this.ExistingFileInfo.Directory.EnumerateFiles(NewFileNameWithExt, SearchOption.TopDirectoryOnly).Count() > 0;
                    }
                    else
                    {
                        this.IsNameConflict = this.ExistingFileInfo.Directory.EnumerateFiles(NewFileName + ".*", SearchOption.TopDirectoryOnly).Count() > 0;
                    }
                }
                else
                    this.IsNameConflict = false;
            }
            catch (IOException ex)
            {
                this.ShowMessageBox(ex.Message, System.Windows.MessageBoxImage.Error);
                this.IsNameConflict = false;
                return;
            }
            if (!this.IsValid)
                return;

            try
            {
                var DestinationFullName = System.IO.Path.Combine(this.ExistingFileInfo.DirectoryName, NewFileNameWithExt);
                this.ExistingFileInfo.MoveTo(DestinationFullName);

                this.RaiseRequestClose(true);
            }
            catch (IOException ex)
            {
                var ErrorMessage = "A file operation error has occurred: " + Environment.NewLine + ex.Message;
                this.ShowMessageBox(ErrorMessage, System.Windows.MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                this.ShowMessageBox(ex.Message, System.Windows.MessageBoxImage.Error);
            }
        }

        protected override void RaiseRequestClose(bool? DialogResult)
        {
            if (this.CancellationTokenSource != null && this.CancellationTokenSource.Token.CanBeCanceled)
                this.CancellationTokenSource.Cancel();

            base.RaiseRequestClose(DialogResult);
        }
        protected override string GetWindowTitle()
        {
            return "Rename File";
        }

        protected override void OnDispose()
        {
            try
            {
                if (this.CancellationTokenSource != null)
                    this.CancellationTokenSource.Dispose();
            }
            catch { }
            this.CancellationTokenSource = null;
            this._ExistingFileInfo = null;
            base.OnDispose();
        }

        #endregion

    }
}