using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFDemo.Presentation.Controls.UpDown
{
    /// <summary>
    /// Base class for an UpDown control.
    /// </summary>
    public abstract class UpDownBase : System.Windows.Controls.Control
    {

        #region Constants

        /// <summary>
        /// Template part name for the text box.
        /// </summary>
        public const string PART_TextBox = "PART_TextBox";
        /// <summary>
        /// Template part name for the spinner.
        /// </summary>
        public const string PART_Spinner = "PART_Spinner";

        #endregion

        #region Static Members

        /// <summary>
        /// Property for setting the ReadOnly status
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly", typeof(bool), typeof(UpDownBase), new UIPropertyMetadata(false, IsReadOnlyProperty_PropertyChanged)
            );
        /// <summary>
        /// Property for setting the text box alignment
        /// </summary>
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
            "TextAlignment", typeof(TextAlignment), typeof(UpDownBase), new UIPropertyMetadata(TextAlignment.Right)
            );

        private static void IsReadOnlyProperty_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var UpDown = o as UpDownBase;
            if (UpDown == null)
                return;

            UpDown.OnIsReadOnlyChanged(e);
        }

        #endregion

        #region Properties

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        #endregion

        #region Constructor

        protected UpDownBase()
        {

        }

        #endregion

        #region Methods

        protected virtual void OnIsReadOnlyChanged(DependencyPropertyChangedEventArgs e) { }

        #endregion

    }
}