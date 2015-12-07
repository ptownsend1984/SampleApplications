using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ImageOrganizer.Common.Utils
{
    public static class ImageFileUtils
    {

        #region Static Variables

        private static readonly string[] SkipExtensions = new string[] 
        {
            ".db"
            ,
            ".avi"
            ,
            ".mkv"
            ,
            ".mov"
            ,
            ".ini"
        };

        #endregion

        #region Static Methods

        public static bool IsSkippedExtension(string Extension)
        {
            return SkipExtensions.Contains(Extension, StringComparer.OrdinalIgnoreCase);
        }

        public static bool IsLongFileName(string FullFilePath)
        {
            if (FullFilePath == null)
                return false;
            return FullFilePath.Length >= 260;
        }

        public static System.Windows.Media.Imaging.BitmapDecoder CreateDecoder(System.IO.FileInfo FileInfo)
        {
            if (FileInfo == null)
                return null;
            if (ImageFileUtils.IsSkippedExtension(FileInfo.Extension))
                return null;
            try
            {
                var FileBytes = System.IO.File.ReadAllBytes(FileInfo.FullName);
                var MemoryStream = new System.IO.MemoryStream(FileBytes);
                return System.Windows.Media.Imaging.BitmapDecoder.Create(
                    MemoryStream
                    , System.Windows.Media.Imaging.BitmapCreateOptions.DelayCreation
                    , System.Windows.Media.Imaging.BitmapCacheOption.OnDemand
                    );
            }
            catch (NotSupportedException)
            {
                //Ignore NotSupportedException
                return null;
            }
            catch (System.IO.FileFormatException)
            {
                //Ignore FileFormatException
                return null;
            }
            catch (System.IO.FileNotFoundException)
            {
                //Throw out a FileNotFoundException
                throw;
            }
            catch (System.IO.IOException)
            {
                //Ignore IOException
                return null;
            }
        }

        #endregion

    }
}