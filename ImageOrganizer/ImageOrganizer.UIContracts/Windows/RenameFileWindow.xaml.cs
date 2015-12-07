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
using ImageOrganizer.Presentation;
using System.ComponentModel.Composition;
using ImageOrganizer.Contracts.ViewModels;
using System.Windows.Controls.Primitives;
using ImageOrganizer.Common.Extensions;

namespace ImageOrganizer.Contracts.Windows
{
    public partial class RenameFileWindow : ImageOrganizerWindow
    {

        #region Properties


        #endregion

        #region Constructor

        public RenameFileWindow()
        {
            InitializeComponent();

            //Oddity with using SizeToContent: Doing this in the loaded event does not work.
            this.SourceInitialized += (s, e) =>
                {
                    var EditableTextBox = this.NewFileNameTextBox.FindVisualChild<TextBoxBase>();
                    if (EditableTextBox != null)
                    {
                        EditableTextBox.SelectAll();
                    }
                    this.NewFileNameTextBox.Focus();
                };
        }

        #endregion

    }
}
