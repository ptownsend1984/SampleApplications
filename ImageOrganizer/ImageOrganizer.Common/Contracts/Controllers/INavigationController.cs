using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace ImageOrganizer.Common.Contracts.Controllers
{
    public interface INavigationController
    {

        #region Methods

        FileInfo GoToImageName(IEnumerable<FileInfo> ActiveFiles);

        #endregion

    }
}