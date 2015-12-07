using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFDemo.Presentation.Controls.UpDown
{
    /// <summary>
    /// UpDown control class for integer values.
    /// </summary>
    public class IntegerUpDown : NumericUpDownBase<int>
    {

        #region Static Members

        static IntegerUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IntegerUpDown), new FrameworkPropertyMetadata(typeof(IntegerUpDown)));
            IncrementProperty.OverrideMetadata(typeof(IntegerUpDown), new FrameworkPropertyMetadata(1));
            MaximumValueProperty.OverrideMetadata(typeof(IntegerUpDown), new FrameworkPropertyMetadata(100));
            MinimumValueProperty.OverrideMetadata(typeof(IntegerUpDown), new FrameworkPropertyMetadata(0));
        }

        #endregion

        #region Methods

        protected override int CoerceValue(int BaseValue)
        {
            if (BaseValue > this.MaximumValue)
                return this.MaximumValue;
            else if (BaseValue < this.MinimumValue)
                return this.MinimumValue;
            else
                return BaseValue;
        }
        protected override int CoerceMaximumValue(int BaseValue)
        {
            if (BaseValue < this.MinimumValue)
                return this.MinimumValue;
            else
                return BaseValue;
        }
        protected override int CoerceMinimumValue(int BaseValue)
        {
            if (BaseValue > this.MaximumValue)
                return this.MaximumValue;
            else
                return BaseValue;
        }

        protected override void DoIncrement()
        {
            this.Value += this.Increment;
        }
        protected override void DoDecrement()
        {
            this.Value -= this.Increment;
        }

        protected override bool CanDoDefaultUpCommand()
        {
            return base.CanDoDefaultUpCommand() && this.Value < this.MaximumValue;
        }
        protected override bool CanDoDefaultDownCommand()
        {
            return base.CanDoDefaultDownCommand() && this.Value > this.MinimumValue;
        }

        #endregion

    }
}