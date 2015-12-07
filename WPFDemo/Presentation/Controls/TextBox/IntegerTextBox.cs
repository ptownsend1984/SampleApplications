using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFDemo.Presentation.Controls.TextBox
{
    public class IntegerTextBox : System.Windows.Controls.TextBox
    {

        static void IntegerTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var Sender = sender as IntegerTextBox;
            if (Sender == null)
                return;

            DataObject.RemovePastingHandler(Sender, IntegerTextBox_PastingHandler);
            DataObject.AddPastingHandler(Sender, IntegerTextBox_PastingHandler);
        }
        static void IntegerTextBox_Unloaded(object sender, RoutedEventArgs e)
        {
            var Sender = sender as IntegerTextBox;
            if (Sender == null)
                return;

            DataObject.RemovePastingHandler(Sender, IntegerTextBox_PastingHandler);
        }
        private static void IntegerTextBox_PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            var Sender = sender as IntegerTextBox;
            if (Sender == null)
                return;

            Sender.OnPaste(e);
        }

        #region Properties

        public int TextLength { get { return !string.IsNullOrEmpty(this.Text) ? this.Text.Length : 0; } }

        #endregion

        #region Constructor

        public IntegerTextBox()
        {
            //Class handlers do not work right with Loaded and Unloaded.  Use instance handlers instead.
            this.AddHandler(LoadedEvent, new RoutedEventHandler(IntegerTextBox_Loaded), true);
            this.AddHandler(UnloadedEvent, new RoutedEventHandler(IntegerTextBox_Unloaded), true);
        }

        #endregion

        #region Event Handlers


        #endregion

        #region Methods

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
                e.Handled = true;
            else
                base.OnPreviewKeyDown(e);
        }
        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e)
        {
            if (CheckTextFormat(e.Text))
                base.OnPreviewTextInput(e);
            else
                e.Handled = true;
        }
        protected virtual void OnPaste(DataObjectPastingEventArgs e)
        {
            var IsTextPresent = e.SourceDataObject.GetDataPresent(System.Windows.DataFormats.Text, true);
            if (IsTextPresent)
            {
                var TextValue = e.SourceDataObject.GetData(DataFormats.Text) as string;
                if (CheckTextFormat(TextValue))
                    return;
            }
            e.CancelCommand();
        }
        protected virtual bool CheckTextFormat(string Value)
        {
            return Value.ToCharArray().All((c) => { return Char.IsNumber(c); });
        }

        #endregion

    }
}