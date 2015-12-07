using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Text.RegularExpressions;
using System.Globalization;
using WPFDemo.Common.Extensions;

namespace WPFDemo.Presentation.Controls.TextBox
{
    //Use in conjunction with the CurrencyStringConverter.  Do not use PropertyChanged updating.
    public class CurrencyTextBox : DecimalTextBox
    {

        #region Methods

        protected override bool TryGetDecimalValue(string text, out decimal value)
        {
            return decimal.TryParse(text, NumberStyles.Currency, CultureInfo.CurrentCulture, out value);
        }
        protected override string ToPrecisionString(decimal value, uint scale)
        {
            //For the precision string, knock it to the lesser of the scale and 2 places.

            //Problem case: 
            //DECIMAL(15,5) rounding 9999999999.99999 takes it to 10000000000,
            //which is out of precision.                                    
            //So we want to truncate the extra places instead of letting it round.
            return (Math.Truncate(value * 100M) / 100M).ToPrecisionCurrencyString(Math.Min(scale, 2));
        }

        #endregion

    }
}