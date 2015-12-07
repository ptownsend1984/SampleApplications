using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Reflection.Helper2;
using Reflection.Helper2.Wrappers;

namespace Reflection.Controls
{
    public partial class TypesListBoxControl : UserControl
    {

        public static readonly DependencyProperty TypesProperty = DependencyProperty.Register(
            "Types", typeof(IEnumerable<TypeWrapper>), typeof(TypesListBoxControl), new FrameworkPropertyMetadata(null));

        public IEnumerable<TypeWrapper> Types
        {
            get { return (IEnumerable<TypeWrapper>)GetValue(TypesProperty); }
            set { SetValue(TypesProperty, value); }
        }

        public TypesListBoxControl()
        {
            InitializeComponent();
        }

    }
}
