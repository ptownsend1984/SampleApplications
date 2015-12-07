using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using ImageOrganizer.Common.Utils;

namespace ImageOrganizer.Presentation.Behaviors
{
    public static class WindowExtensions
    {

        #region Static Properties

        public static readonly DependencyProperty PreventOffWorkingAreaProperty = DependencyProperty.RegisterAttached(
            "PreventOffWorkingArea", typeof(bool), typeof(WindowExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, PreventOffWorkingArea_PropertyChanged)
            );


        #endregion

        #region Static Methods

        public static bool GetPreventOffWorkingArea(Window element)
        {
            return (bool)element.GetValue(PreventOffWorkingAreaProperty);
        }
        public static void SetPreventOffWorkingArea(Window element, bool Value)
        {
            element.SetValue(PreventOffWorkingAreaProperty, Value);
        }

        private static void PreventOffWorkingArea_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Window = o as Window;
            if (Window == null)
                return;

            var IsSet = (bool)e.NewValue;

            Window.SourceInitialized -= Window_SourceInitialized;
            if (IsSet)
            {
                Window.SourceInitialized += Window_SourceInitialized;
            }
        }

        static void Window_SourceInitialized(object sender, EventArgs e)
        {
            var Window = sender as Window;
            if (Window == null)
                return;
            try
            {
                var WorkingArea = WinAPIUtils.GetWindowWorkspace(Window);
                if (WorkingArea.Width == 0)
                    return;
                if (Window.Left < WorkingArea.Left)
                    Window.Left = WorkingArea.Left;
                if (Window.Top < WorkingArea.Top)
                    Window.Top = WorkingArea.Top;
            }
            finally
            {
                Window.SourceInitialized -= Window_SourceInitialized;
            }
        }

        #endregion

    }
}