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
using System.IO;
using System.Collections.Specialized;
using ImageOrganizer.Common;
using System.Windows.Media.Imaging;
using ImageOrganizer.ViewModels.Images;
using ImageOrganizer.Common.Contracts.Controllers;
using ImageOrganizer.Common.Extensions;

namespace ImageOrganizer.ViewModels
{
    public class ActiveImagesViewModel : ViewModel
    {

        #region Constants

        //Max 800%
        private const double MaxZoomIn = 8d;
        //Min .5%
        private const double MaxZoomOut = .005d;

        #endregion

        #region Static Members


        #endregion

        #region Global Variables

        private SyncObservableCollection<FileInfo> _ActiveFiles;
        private SyncListCollectionView _ActiveFilesView;
        private readonly object ActiveFilesLock = new object();

        private ImageViewModel _ActiveImage;
        private readonly object ActiveImageLock = new object();

        private int _IsLoadingFiles;
        private CancellationTokenSource ActiveCancellationTokenSource;

        private double _ViewerWidth;
        private double _ViewerHeight;

        private double _ZoomPercent;

        #endregion

        #region Properties

        public double ViewerWidth
        {
            get { return _ViewerWidth; }
            set
            {
                _ViewerWidth = value;
                this.RefreshActiveImage();
            }
        }
        public double ViewerHeight
        {
            get { return _ViewerHeight; }
            set
            {
                _ViewerHeight = value;
                this.RefreshActiveImage();
            }
        }
        public double ZoomPercent
        {
            get { return _ZoomPercent; }
            set
            {
                if (value <= 0d)
                {
                    _ZoomPercent = 1;
                }
                else
                {
                    _ZoomPercent = value;
                }
                RaisePropertyChanged(() => this.ZoomPercent);
            }
        }
        private ZoomType ZoomType { get; set; }

        public IEnumerable<FileInfo> ActiveFiles { get { return _ActiveFiles; } }
        internal SyncObservableCollection<FileInfo> ActiveFilesCollection { get { return _ActiveFiles; } }
        public IEnumerable ActiveFilesView { get { return _ActiveFilesView; } }
        public int Position { get { return _ActiveFilesView.CurrentPosition; } }
        public int DisplayPosition { get { return Position + 1; } }

        public bool IsViewingFiles { get { return this._ActiveFiles.Count > 0; } }
        public bool IsViewingFirstFile { get { return this.IsViewingFiles && this._ActiveFilesView.CurrentPosition == 0; } }
        public bool IsViewingLastFile { get { return this.IsViewingFiles && this._ActiveFilesView.CurrentPosition == this._ActiveFiles.Count - 1; } }

        public ImageViewModel ActiveImage
        {
            get { return this._ActiveImage; }
            set
            {
                if (_ActiveImage != value)
                {
                    _ActiveImage = value;
                    RaisePropertyChanged(() => this.ActiveImage);
                }
            }
        }

        public bool IsLoadingFiles
        {
            get { return _IsLoadingFiles > 0; }
            set
            {
                if (value)
                    Interlocked.Increment(ref _IsLoadingFiles);
                else
                    Interlocked.Decrement(ref _IsLoadingFiles);
                RaisePropertyChanged(() => this.IsLoadingFiles);
            }
        }
        private bool IsClosingFolder { get; set; }

        #endregion

        #region Events

        public event EventHandler ActiveImageChanged;

        #endregion

        #region Constructor

        public ActiveImagesViewModel()
        {
            _ActiveFiles = new SyncObservableCollection<FileInfo>();
            _ActiveFilesView = new SyncListCollectionView(_ActiveFiles);
            _ActiveFilesView.CustomSort = new NaturalStringComparer();

            _ActiveFiles.CollectionChanged += ActiveFiles_CollectionChanged;
            _ActiveFilesView.CurrentChanging += ActiveFilesView_CurrentChanging;
            _ActiveFilesView.CurrentChanged += ActiveFilesView_CurrentChanged;

            this.ZoomType = ImageOrganizer.Common.ZoomType.BestFit;
        }

        #endregion

        #region Event Handlers

        private void ActiveFiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaiseSelectedImageProperties();
        }
        private void ActiveFilesView_CurrentChanging(object sender, CurrentChangingEventArgs e)
        {
        }
        private void ActiveFilesView_CurrentChanged(object sender, EventArgs e)
        {
            //Limitation: Cannot change the current item while in the CurrentChanged event
            RaiseSelectedImageProperties();
            RaisePositionProperties();

            this.RaiseEvent(this.ActiveImageChanged);
        }

        #endregion

        #region Methods

        internal bool CancelLoadImages()
        {
            if (this.IsLoadingFiles)
            {
                var ActiveCancelTokenSource = this.ActiveCancellationTokenSource;
                if (ActiveCancelTokenSource != null)
                {
                    ActiveCancelTokenSource.Cancel();
                    return true;
                }
            }
            return false;
        }
        internal void CloseFolder()
        {
            System.Threading.Tasks.Task.Factory.StartNew(() => CloseFolderCore());
        }
        private void CloseFolderCore()
        {
            if (this.IsClosingFolder)
                return;
            this.IsClosingFolder = true;
            try
            {
                this.CancelLoadImages();
                using (var ManualResetEvent = new ManualResetEvent(false))
                {
                    var WaitThread = System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        WaitForStopLoading();
                        try
                        {
                            ManualResetEvent.Set();
                        }
                        catch { }
                    });
                    ManualResetEvent.WaitOne(new TimeSpan(0, 0, 10));
                }
                lock (ActiveFilesLock)
                {
                    _ActiveFiles.Clear();
                }
                lock (ActiveImageLock)
                {
                    this.ActiveImage = null;
                }
            }
            finally
            {
                this.IsClosingFolder = false;
            }
        }
        private void WaitForStopLoading()
        {
            while (this.IsLoadingFiles)
                Thread.Sleep(1);
        }

        internal void LoadImages(string DirectoryPath)
        {
            this.CancelLoadImages();

            var TokenSource = new CancellationTokenSource();
            var Task = System.Threading.Tasks.Task.Factory.StartNew(
                (Action<object>)delegate { LoadImagesCore(DirectoryPath, TokenSource.Token); }, DirectoryPath, TokenSource.Token
                );
            this.ActiveCancellationTokenSource = TokenSource;
        }
        private void LoadImagesCore(string DirectoryPath, CancellationToken CancellationToken)
        {
            this.IsLoadingFiles = true;
            try
            {
                CancellationToken.ThrowIfCancellationRequested();

                _ActiveFiles.Clear();
                this.ActiveImage = null;

                this.ZoomFit();

                System.IO.DirectoryInfo SelectedDirectory;
                try
                {
                    SelectedDirectory = new System.IO.DirectoryInfo(DirectoryPath);
                }
                catch (System.IO.IOException)
                {
                    //System.Windows.MessageBox.Show("Could not open directory: " + ex.Message);
                    return;
                }

                var NaturalStringComparer = new NaturalStringComparer();
                foreach (var FileInfo in SelectedDirectory.EnumerateFiles().OrderBy((o) => o.Name, NaturalStringComparer))
                {
                    CancellationToken.ThrowIfCancellationRequested();

                    var Decoder = ImageFileUtils.CreateDecoder(FileInfo);
                    if (Decoder == null || Decoder.Frames == null || Decoder.Frames.Count == 0 || Decoder.Frames[0].Width == 0 || Decoder.Frames[0].Height == 0)
                        continue;

                    lock (ActiveFilesLock)
                    {
                        _ActiveFiles.Add(FileInfo);
                    }
                    if (_ActiveFiles.Count == 1)
                        this.MoveToFirst();
                }
            }
            catch (OperationCanceledException)
            {
                //Ignore task cancel
                throw;
            }
            catch (Exception)
            {
                //TODO: Error handling
            }
            finally
            {
                this.IsLoadingFiles = false;
            }
        }

        //Handle movement by preloading the next image, if one        
        internal void MoveToFirst()
        {
            if (!this.IsViewingFiles || this.IsViewingFirstFile)
                return;

            MoveToFile(0);
        }
        internal void MoveToPrevious()
        {
            if (!this.IsViewingFiles || this.IsViewingFirstFile)
                return;

            MoveToFile(_ActiveFilesView.CurrentPosition - 1);
        }
        internal void MoveToNext()
        {
            if (!this.IsViewingFiles || this.IsViewingLastFile)
                return;

            MoveToFile(_ActiveFilesView.CurrentPosition + 1);
        }
        internal void MoveToLast()
        {
            if (!this.IsViewingFiles || this.IsViewingLastFile)
                return;

            MoveToFile(_ActiveFiles.Count - 1);
        }
        internal void MoveToImageName()
        {
            if (!this.IsViewingFiles)
                return;

            var Controller = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<INavigationController>();
            var Result = Controller.GoToImageName(this.ActiveFiles);
            if (Result == null)
                return;
            var ResultPosition = this._ActiveFilesView.IndexOf(Result);
            if (ResultPosition < 0 || ResultPosition == this.Position)
                return;
            MoveToFile(ResultPosition);
        }
        private void MoveToFile(int NextPosition)
        {
            FileInfo FileInfo = null;
            while (this._ActiveFiles.Count > 0 && NextPosition > -1 && NextPosition < this._ActiveFiles.Count && FileInfo == null)
            {
                FileInfo = this._ActiveFilesView.GetItemAt(NextPosition) as FileInfo;
                if (!PreloadFile(FileInfo))
                {
                    this._ActiveFiles.Remove(FileInfo);
                    FileInfo = null;
                    if (NextPosition >= this._ActiveFiles.Count)
                        NextPosition--;
                }
            }
            if (FileInfo != null)
                this._ActiveFilesView.MoveCurrentTo(FileInfo);
        }
        private bool PreloadFile(FileInfo File)
        {
            try
            {
                this.Invoke(new Action<FileInfo>(CreateActiveImage), File);
                return true;
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                if (!(ex.ExceptionTypeWas(typeof(System.IO.FileNotFoundException))))
                    throw;
            }
            catch (System.IO.FileNotFoundException)
            {
                //Ignore FileNotFoundException
            }
            return false;
        }

        internal void ZoomIn()
        {
            this.ZoomType = ImageOrganizer.Common.ZoomType.Zoom;
            var ZoomPercent = this.ZoomPercent;
            ZoomPercent *= 1.5d;
            if (ZoomPercent > MaxZoomIn)
                ZoomPercent = MaxZoomIn;
            this.ZoomPercent = ZoomPercent;
            this.RefreshActiveImage();
        }
        internal void ZoomOut()
        {
            this.ZoomType = ImageOrganizer.Common.ZoomType.Zoom;
            var ZoomPercent = this.ZoomPercent;
            ZoomPercent /= 1.5d;
            if (ZoomPercent < MaxZoomOut)
                ZoomPercent = MaxZoomOut;
            this.ZoomPercent = ZoomPercent;
            this.RefreshActiveImage();
        }
        internal void ActualSize()
        {
            this.ZoomType = ImageOrganizer.Common.ZoomType.Zoom;
            this.ZoomPercent = 1d;
            this.RefreshActiveImage();
        }
        internal void ZoomFit()
        {
            this.ZoomType = ImageOrganizer.Common.ZoomType.BestFit;
            this.RefreshActiveImage();
        }
        internal void FitWidth()
        {
            this.ZoomType = ImageOrganizer.Common.ZoomType.FitWidth;
            this.RefreshActiveImage();
        }
        internal void FitHeight()
        {
            this.ZoomType = ImageOrganizer.Common.ZoomType.FitHeight;
            this.RefreshActiveImage();
        }

        internal void RenameActiveImage()
        {
            if (!this.IsViewingFiles || this.IsLoadingFiles)
                return;
            var ActiveImage = this.ActiveImage;
            if (ActiveImage == null)
                return;

            var Controller = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IFileOperationsController>();
            Controller.RenameFile(ActiveImage);
        }
        internal void DeleteActiveImage()
        {
            if (!this.IsViewingFiles || this.IsLoadingFiles)
                return;
            var ActiveImage = this.ActiveImage;
            if (ActiveImage == null)
                return;

            var Controller = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IFileOperationsController>();
            var Result = Controller.DeleteFile(ActiveImage.FileInfo);
            if (!Result)
                return;

            int NextPosition = -1;
            if (this._ActiveFiles.Count > 1)
            {
                if (IsViewingFirstFile)
                    NextPosition = 0;
                else if (IsViewingLastFile)
                    NextPosition = this._ActiveFiles.Count - 2;
                else
                    NextPosition = this.Position;
            }
            this._ActiveFiles.Remove(ActiveImage.FileInfo);
            if (NextPosition > -1)
                this.MoveToFile(NextPosition);
        }

        private void CreateActiveImage(FileInfo FileInfo)
        {
            if (FileInfo == null)
                return;
            lock (ActiveImageLock)
            {
                var Decoder = this.Invoke<BitmapDecoder>(new Func<FileInfo, BitmapDecoder>(ImageFileUtils.CreateDecoder), FileInfo);
                this.ActiveImage = this.Invoke<ImageViewModel>(new Func<FileInfo, BitmapDecoder, ImageViewModel>(CreateImageViewModel), FileInfo, Decoder);
            }
            RefreshActiveImage();
        }
        internal void RefreshActiveImage()
        {
            RefreshImage(this.ActiveImage);
        }
        private void RefreshImage(ImageViewModel Image)
        {
            if (Image == null)
                return;

            var BitmapSource = this.Invoke<BitmapSource>(new Func<BitmapDecoder, BitmapSource>(CreateBitmapSource), Image.BitmapDecoder);
            Image.Source = BitmapSource;
            switch (this.ZoomType)
            {
                case ZoomType.BestFit:
                case ZoomType.FitWidth:
                case ZoomType.FitHeight:
                    this._ZoomPercent = Image.Zoom;
                    break;
            }
        }
        private ImageViewModel CreateImageViewModel(FileInfo FileInfo, BitmapDecoder Decoder)
        {
            return new ImageViewModel(FileInfo, Decoder);
        }
        private BitmapSource CreateBitmapSource(BitmapDecoder Decoder)
        {
            BitmapSource Source = null;
            try
            {
                var Frame = Decoder.Frames[0];
                if (Frame.CanFreeze && !Frame.IsFrozen)
                    Frame.Freeze();
                switch (this.ZoomType)
                {
                    case ZoomType.Zoom:
                        Source = new TransformedBitmap(Frame, new ScaleTransform(this.ZoomPercent, this.ZoomPercent));
                        break;
                    case ZoomType.BestFit:
                        if (Frame.PixelWidth < this.ViewerWidth && Frame.PixelHeight < this.ViewerHeight)
                            Source = Frame;
                        else if (Frame.PixelWidth >= this.ViewerWidth && Frame.PixelHeight >= this.ViewerHeight)
                        {
                            var dw = this.ViewerWidth / (double)Frame.PixelWidth;
                            var dh = this.ViewerHeight / (double)Frame.PixelHeight;
                            var delta = Math.Min(dw, dh);
                            Source = new TransformedBitmap(Frame, new ScaleTransform(delta, delta));
                        }
                        else
                        {
                            double dw;
                            double dh;
                            if (Frame.PixelWidth < this.ViewerWidth)
                            {
                                dh = this.ViewerHeight / (double)Frame.PixelHeight;
                                dw = dh;
                            }
                            else
                            {
                                dw = this.ViewerWidth / (double)Frame.PixelWidth;
                                dh = dw;
                            }
                            Source = new TransformedBitmap(Frame, new ScaleTransform(dw, dh));
                        }
                        break;
                    default:
                        throw new NotImplementedException(this.ZoomType.ToString());
                }
                return Source;
            }
            finally
            {
                if (Source != null)
                {
                    if (Source.CanFreeze && !Source.IsFrozen)
                        Source.Freeze();
                }
            }
        }

        private void RaiseSelectedImageProperties()
        {
            this.RaisePropertyChanged(() => this.IsViewingFiles);
            this.RaisePropertyChanged(() => this.IsViewingFirstFile);
            this.RaisePropertyChanged(() => this.IsViewingLastFile);
        }
        private void RaisePositionProperties()
        {
            this.RaisePropertyChanged(() => this.Position);
            this.RaisePropertyChanged(() => this.DisplayPosition);
        }

        #endregion

    }
}