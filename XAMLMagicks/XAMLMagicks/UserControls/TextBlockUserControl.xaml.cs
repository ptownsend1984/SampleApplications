using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace XAMLMagicks.UserControls
{

    public partial class TextBlockUserControl : UserControl
    {

        #region Static Members
        
        public static DependencyProperty TextProperty =
            DependencyProperty.Register(
            "Text", typeof(string), typeof(TextBlockUserControl),
            new PropertyMetadata(null)
            );

        #endregion

        #region Properties

        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        #endregion

        #region Constructor

        public TextBlockUserControl()
        {
            InitializeComponent();
        }

        #endregion

    }
}
