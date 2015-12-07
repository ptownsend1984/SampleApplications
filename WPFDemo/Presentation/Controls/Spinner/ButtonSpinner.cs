using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Input;

namespace WPFDemo.Presentation.Controls.Spinner
{
    /// <summary>
    /// Spinner class that uses an up button and a down button.
    /// </summary>
    /// <remarks>The point of using ICommand and buttons is to let WPF handle enabling the up and down buttons.</remarks>
    [TemplatePart(Name = PART_UpButton, Type = typeof(System.Windows.Controls.Primitives.ButtonBase))]
    [TemplatePart(Name = PART_DownButton, Type = typeof(System.Windows.Controls.Primitives.ButtonBase))]
    [ContentProperty("Content")]
    public class ButtonSpinner : SpinnerBase
    {

        #region Constants

        public const string PART_UpButton = "PART_UpButton";
        public const string PART_DownButton = "PART_DownButton";

        #endregion

        #region Static Members

        static ButtonSpinner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonSpinner), new FrameworkPropertyMetadata(typeof(ButtonSpinner)));
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(
            "Content", typeof(object), typeof(ButtonSpinner), new UIPropertyMetadata(null, ContentProperty_PropertyChanged)
            );

        private static void ContentProperty_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var ButtonSpinner = o as ButtonSpinner;
            if (ButtonSpinner == null)
                return;

            ButtonSpinner.OnContentPropertyChanged(e);
        }

        #endregion

        #region Global Variables

        private System.Windows.Controls.Primitives.ButtonBase _UpButton;
        private System.Windows.Controls.Primitives.ButtonBase _DownButton;

        #endregion

        #region Properties

        public System.Windows.Controls.Primitives.ButtonBase UpButton
        {
            get { return _UpButton; }
            private set
            {
                if (_UpButton != null)
                {
                    _UpButton.Command = null;
                }
                _UpButton = value;
                if (_UpButton != null)
                {
                    _UpButton.Command = this.UpCommand;
                }
            }
        }
        public System.Windows.Controls.Primitives.ButtonBase DownButton
        {
            get { return _DownButton; }
            private set
            {
                if (_DownButton != null)
                {
                    _DownButton.Command = null;
                }
                _DownButton = value;
                if (_DownButton != null)
                {
                    _DownButton.Command = this.DownCommand;
                }
            }
        }

        public object Content
        {
            get { return GetValue(ContentProperty) as object; }
            set { SetValue(ContentProperty, value); }
        }

        #endregion

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.UpButton = GetTemplateChild(PART_UpButton) as System.Windows.Controls.Primitives.ButtonBase;
            this.DownButton = GetTemplateChild(PART_DownButton) as System.Windows.Controls.Primitives.ButtonBase;
        }

        protected override void OnUpCommandPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnUpCommandPropertyChanged(e);

            var UpButton = this.UpButton;
            if (UpButton == null)
                return;

            UpButton.Command = e.NewValue as ICommand;
        }
        protected override void OnDownCommandPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnDownCommandPropertyChanged(e);

            var DownButton = this.DownButton;
            if (DownButton == null)
                return;

            DownButton.Command = e.NewValue as ICommand;
        }

        protected virtual void OnContentPropertyChanged(DependencyPropertyChangedEventArgs e) { }

        #endregion

    }
}