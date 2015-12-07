using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImageOrganizer.Common.Contracts.Images;
using System.IO;

namespace ImageOrganizer.Common.Contracts.Controllers
{
    public interface IFileOperationsController
    {

        #region Properties


        #endregion

        #region Events


        #endregion

        #region Methods

        void RenameFile(IImageViewModel ImageViewModel);
        bool DeleteFile(FileInfo FileInfo);

        #endregion

    }
}