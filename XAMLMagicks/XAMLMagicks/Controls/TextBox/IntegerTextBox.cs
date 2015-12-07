using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace XAMLMagicks.Controls.TextBox
{
    public class IntegerTextBox : System.Windows.Controls.TextBox
    {

        #region Properties

        public int TextLength { get { return !string.IsNullOrEmpty(this.Text) ? this.Text.Length : 0; } }

        #endregion

        #region Constructor

        public IntegerTextBox()
        {
            this.Loaded += (s, e) =>
            {
                DataObject.RemovePastingHandler(this, this_PastingHandler);
                DataObject.AddPastingHandler(this, this_PastingHandler);
            };
            this.Unloaded += (s, e) =>
            {
                DataObject.RemovePastingHandler(this, this_PastingHandler);
            };
        }

        #endregion

        #region Event Handlers

        private void this_PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            OnPaste(e);
        }

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