using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFDemo.Presentation.Common
{
    
    public class DependencyPropertyChangedEventArgsEx : EventArgs
    {
        public DependencyPropertyChangedEventArgsEx(DependencyProperty property, object oldValue, object newValue)
        {
            this.Property = property;
            this.NewValue = newValue;
            this.OldValue = oldValue;
        }

        public object NewValue { get; private set; }
        public object OldValue { get; private set; }
        public DependencyProperty Property { get; private set; }
    }
}