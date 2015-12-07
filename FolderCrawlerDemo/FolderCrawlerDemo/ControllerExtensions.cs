using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace FolderCrawlerDemo
{
    [DebuggerStepThrough]
    public static class ControllerExtensions
    {

        public static MessageBoxResult ShowMessageBox(this UINotificationObject controller, string message)
        {
            return ShowMessageBox(controller, message, MessageBoxButton.OK, MessageBoxImage.None);
        }
        public static MessageBoxResult ShowErrorMessageBox(this UINotificationObject controller, Exception ex)
        {
            return ShowErrorMessageBox(controller, ex.ToString());
        }
        public static MessageBoxResult ShowErrorMessageBox(this UINotificationObject controller, string message)
        {
            return ShowMessageBox(controller, message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static MessageBoxResult ShowMessageBox(this UINotificationObject controller, string message, MessageBoxButton buttons, MessageBoxImage icon)
        {
            return MessageBox.Show(controller.Owner, message, controller.FormText, buttons, icon);
        }
    }
}