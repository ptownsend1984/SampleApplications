using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace XAMLMagicks.Controls.Spinner
{
    /// <summary>
    /// Abstract base class for a spinner control.
    /// </summary>
    public abstract class SpinnerBase : System.Windows.Controls.Control
    {

        #region Static Members

        /// <summary>
        /// ICommand property for the Up command.
        /// </summary>
        public static readonly DependencyProperty UpCommandProperty =
            DependencyProperty.Register(
            "UpCommand", typeof(ICommand), typeof(SpinnerBase), new UIPropertyMetadata(null, UpCommandProperty_PropertyChanged)
            );
        /// <summary>
        /// Command parameter for the Up command.
        /// </summary>
        public static readonly DependencyProperty UpCommandParameterProperty =
            DependencyProperty.Register(
            "UpCommandParameter", typeof(object), typeof(SpinnerBase), new UIPropertyMetadata(null)
            );
        /// <summary>
        /// ICommand property for the Down command.
        /// </summary>
        public static readonly DependencyProperty DownCommandProperty =
            DependencyProperty.Register(
            "DownCommand", typeof(ICommand), typeof(SpinnerBase), new UIPropertyMetadata(null, DownCommandProperty_PropertyChanged)
            );
        /// <summary>
        /// Command parameter for the Down command.
        /// </summary>
        public static readonly DependencyProperty DownCommandParameterProperty =
            DependencyProperty.Register(
            "DownCommandParameter", typeof(object), typeof(SpinnerBase), new UIPropertyMetadata(null)
            );

        private static void UpCommandProperty_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var SpinnerBase = o as SpinnerBase;
            if (SpinnerBase == null)
                return;

            SpinnerBase.OnUpCommandPropertyChanged(e);
        }
        private static void DownCommandProperty_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var SpinnerBase = o as SpinnerBase;
            if (SpinnerBase == null)
                return;

            SpinnerBase.OnDownCommandPropertyChanged(e);
        }

        #endregion

        #region Properties

        public ICommand UpCommand
        {
            get { return (ICommand)GetValue(UpCommandProperty); }
            set { SetValue(UpCommandProperty, value); }
        }
        public object UpCommandParameter
        {
            get { return (object)GetValue(UpCommandParameterProperty); }
            set { SetValue(UpCommandParameterProperty, value); }
        }
        public ICommand DownCommand
        {
            get { return (ICommand)GetValue(DownCommandProperty); }
            set { SetValue(DownCommandProperty, value); }
        }
        public object DownCommandParameter
        {
            get { return (object)GetValue(DownCommandParameterProperty); }
            set { SetValue(DownCommandParameterProperty, value); }
        }

        #endregion

        #region Constructor

        protected SpinnerBase()
        {

        }

        #endregion

        #region Methods

        protected virtual void OnUpCommandPropertyChanged(DependencyPropertyChangedEventArgs e) { }
        protected virtual void OnDownCommandPropertyChanged(DependencyPropertyChangedEventArgs e) { }

        #endregion

    }
}