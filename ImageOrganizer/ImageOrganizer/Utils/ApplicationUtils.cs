using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ImageOrganizer.Utils
{
    public static class ApplicationUtils
    {

        #region Static Global Variables

        private static double _XDpi;
        private static double _YDpi;

        #endregion

        #region Static Properties

        public static double XDpi
        {
            get
            {
                if (_XDpi == 0)
                    GetDpi();
                return _XDpi;
            }
        }
        public static double YDpi
        {
            get
            {
                if (_YDpi == 0)
                    GetDpi();
                return _YDpi;
            }
        }

        #endregion

        #region Static Methods

        private static void GetDpi()
        {
            //http://blogs.msdn.com/b/jaimer/archive/2007/03/07/getting-system-dpi-in-wpf-app.aspx
            var PresentationSource = System.Windows.PresentationSource.FromVisual(System.Windows.Application.Current.MainWindow);
            if (PresentationSource == null || PresentationSource.CompositionTarget == null)
            {
                _XDpi = 96;
                _YDpi = 96;
                return;
            }
            var Matrix = PresentationSource.CompositionTarget.TransformToDevice;
            _XDpi = Matrix.M11 * 96;
            _YDpi = Matrix.M22 * 96;
        }

        #endregion

    }
}