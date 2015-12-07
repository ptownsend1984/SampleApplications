using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using WPFDemo.Common;

namespace WPFDemo.Presentation.Behaviors
{
    public static class SystemExtensions
    {

        //Use these extensions to show different UI based upon the operating system and its bitness

        #region Static Properties

        public static readonly DependencyPropertyKey IsXPOSPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsXPOS", typeof(bool), typeof(SystemExtensions), new PropertyMetadata(SystemUtils.GetIsXPOS())
            );
        public static readonly DependencyProperty IsXPOSProperty = IsXPOSPropertyKey.DependencyProperty;

        public static readonly DependencyPropertyKey IsVistaOSPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsVistaOS", typeof(bool), typeof(SystemExtensions), new PropertyMetadata(SystemUtils.GetIsVistaOS())
            );
        public static readonly DependencyProperty IsVistaOSProperty = IsVistaOSPropertyKey.DependencyProperty;

        public static readonly DependencyPropertyKey IsWindows7OSPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsWindows7OS", typeof(bool), typeof(SystemExtensions), new PropertyMetadata(SystemUtils.GetIsWindows7OS())
            );
        public static readonly DependencyProperty IsWindows7OSProperty = IsWindows7OSPropertyKey.DependencyProperty;

        public static readonly DependencyPropertyKey IsWindows8OSPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsWindows8OS", typeof(bool), typeof(SystemExtensions), new PropertyMetadata(SystemUtils.GetIsWindows8OS())
            );
        public static readonly DependencyProperty IsWindows8OSProperty = IsWindows8OSPropertyKey.DependencyProperty;

        public static readonly DependencyPropertyKey IsServerOSPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsServerOS", typeof(bool), typeof(SystemExtensions), new PropertyMetadata(SystemUtils.GetIsServerOS())
            );
        public static readonly DependencyProperty IsServerOSProperty = IsServerOSPropertyKey.DependencyProperty;

        public static readonly DependencyPropertyKey Is64BitProcessPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "Is64BitProcess", typeof(bool), typeof(SystemExtensions), new PropertyMetadata(SystemUtils.IsRunningAs64Bit())
            );
        public static readonly DependencyProperty Is64BitProcessProperty = Is64BitProcessPropertyKey.DependencyProperty;

        public static readonly DependencyPropertyKey Is64BitOSPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "Is64BitOS", typeof(bool), typeof(SystemExtensions), new PropertyMetadata(SystemUtils.GetIs64BitOS())
            );
        public static readonly DependencyProperty Is64BitOSProperty = Is64BitOSPropertyKey.DependencyProperty;

        #endregion

        #region Static Methods

        public static bool GetIsXPOS(DependencyObject o)
        {
            return (bool)o.GetValue(IsXPOSProperty);
        }

        public static bool GetIsVistaOS(DependencyObject o)
        {
            return (bool)o.GetValue(IsVistaOSProperty);
        }

        public static bool GetIsWindows7OS(DependencyObject o)
        {
            return (bool)o.GetValue(IsWindows7OSProperty);
        }

        public static bool GetIsWindows8OS(DependencyObject o)
        {
            return (bool)o.GetValue(IsWindows8OSProperty);
        }

        public static bool GetIsServerOS(DependencyObject o)
        {
            return (bool)o.GetValue(IsServerOSProperty);
        }

        public static bool GetIs64BitProcess(DependencyObject o)
        {
            return (bool)o.GetValue(Is64BitProcessProperty);
        }

        public static bool GetIs64BitOS(DependencyObject o)
        {
            return (bool)o.GetValue(Is64BitOSProperty);
        }

        #endregion

    }
}