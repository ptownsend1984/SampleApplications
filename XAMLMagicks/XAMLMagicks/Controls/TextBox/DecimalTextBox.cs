using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XAMLMagicks.Controls.TextBox
{
    public class DecimalTextBox : IntegerTextBox
    {

        #region Methods

        protected override bool CheckTextFormat(string Value)
        {
            //Entire text is being replaced
            if (this.SelectionLength == this.TextLength)
            {
                if (!string.IsNullOrEmpty(Value))
                {
                    if (Value.Length == 1 && (Value[0] == '-' || Value[0] == '.'))
                        return true;
                }
                return IsValidDecimalFormat(Value);
            }

            //Replace selection with Value
            string EntireText = this.Text;
            if (this.SelectionLength > 0)
            {
                EntireText = EntireText.Remove(this.SelectionStart, this.SelectionLength);
            }
            EntireText = EntireText.Insert(Math.Min(this.SelectionStart, EntireText.Length), Value);
            return IsValidDecimalFormat(EntireText);
        }
        protected virtual bool IsValidDecimalFormat(string Value)
        {
            decimal Temp;
            return decimal.TryParse(Value, out Temp);
        }

        #endregion

    }
}