using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.IO;
using ImageOrganizer.Common.Contracts.Images;

namespace ImageOrganizer.ViewModels.Images
{
    public class ImageViewModel : ViewModel, IImageViewModel
    {

        #region Global Variables

        private FileInfo _FileInfo;
        private readonly BitmapDecoder _BitmapDecoder;
        private BitmapSource _Source;

        #endregion

        #region Properties

        public FileInfo FileInfo
        {
            get { return _FileInfo; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("FileInfo");
                _FileInfo = value;
                RaisePropertyChanged(() => this.FileInfo);
            }
        }

        public BitmapDecoder BitmapDecoder { get { return this._BitmapDecoder; } } 
        public int OriginalPixelWidth
        {
            get { return this.BitmapDecoder.Frames[0].PixelWidth; }
        }
        public int OriginalPixelHeight
        {
            get { return this.BitmapDecoder.Frames[0].PixelHeight; }
        }

        public BitmapSource Source
        {
            get { return _Source; }
            set
            {
                _Source = value;
                RaisePropertyChanged(() => this.Source);
                RaisePropertyChanged(() => this.Width);
                RaisePropertyChanged(() => this.Height);
                RaisePropertyChanged(() => this.PixelWidth);
                RaisePropertyChanged(() => this.PixelHeight);
                RaisePropertyChanged(() => this.AspectRatio);
                RaisePropertyChanged(() => this.Zoom);
            }
        }

        public double Width
        {
            get
            {
                var Source = this.Source;
                if (Source != null)
                    return Source.Width;
                else
                    return 0;
            }
        }
        public double Height
        {
            get
            {
                var Source = this.Source;
                if (Source != null)
                    return Source.Height;
                else
                    return 0;
            }
        }
        public double PixelWidth
        {
            get
            {
                var Source = this.Source;
                if (Source != null)
                    return Source.PixelWidth;
                else
                    return 0;
            }
        }
        public double PixelHeight
        {
            get
            {
                var Source = this.Source;
                if (Source != null)
                    return Source.PixelHeight;
                else
                    return 0;
            }
        }
        public double AspectRatio
        {
            get { return this.Height > 0 ? this.Width / this.Height : 1; }
        }
        public double Zoom
        {
            get { return Math.Min((double)this.PixelWidth / (double)this.OriginalPixelWidth, (double)this.PixelHeight / (double)this.OriginalPixelHeight); }
        }

        #endregion

        #region Events


        #endregion

        #region Constructor

        public ImageViewModel(FileInfo FileInfo, BitmapDecoder BitmapDecoder)
        {
            if (FileInfo == null)
                throw new ArgumentNullException("FileInfo");
            if (BitmapDecoder == null)
                throw new ArgumentNullException("BitmapDecoder");
            if (BitmapDecoder.Frames == null || BitmapDecoder.Frames.Count == 0)
                throw new ArgumentException("BitmapDecoder contains no frames.", "BitmapDecoder");

            this._FileInfo = FileInfo;
            this._BitmapDecoder = BitmapDecoder;
        }

        #endregion

        #region Event Handlers


        #endregion

        #region Methods


        #endregion

    }
}