using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.IO;

namespace ImageOrganizer.Common.Contracts.Images
{
    public interface IImageViewModel : IViewModel
    {

        #region Properties

        BitmapSource Source { get; set; }
        FileInfo FileInfo { get; set; }

        double Width { get; }
        double Height { get; }
        double PixelWidth { get; }
        double PixelHeight { get; }

        #endregion

        #region Events


        #endregion

        #region Methods


        #endregion

    }
}