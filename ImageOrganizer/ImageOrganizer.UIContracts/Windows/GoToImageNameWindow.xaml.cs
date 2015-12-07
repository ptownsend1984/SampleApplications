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

namespace ImageOrganizer.Contracts.Windows
{

    public partial class GoToImageNameWindow : ImageOrganizerWindow
    {

        #region Subclasses


        #endregion

        #region Constants


        #endregion

        #region Static Members


        #endregion

        #region Global Variables


        #endregion

        #region Properties


        #endregion

        #region Events


        #endregion

        #region Constructor

        public GoToImageNameWindow()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
                {
                    this.DestinationControl.Focus();
                };
        }

        #endregion

        #region Event Handlers


        #endregion

        #region Methods


        #endregion

    }
}
