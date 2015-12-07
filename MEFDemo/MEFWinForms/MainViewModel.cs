using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MEFContracts;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using MEFWinForms;

namespace MEFPresentation
{
    [Export(typeof(IMainViewModel))]
    public class MainViewModel : NotificationObject, IMainViewModel
    {

        private string _Message;
        private readonly DelegateCommand _OKCommand;

        public MainViewModel()
        {
            _Message = "This is a message for you";
            _OKCommand = new DelegateCommand(DoOKCommand);
        }

        public string Message
        {
            get { return _Message; }
            set
            {
                if (_Message != value)
                {
                    _Message = value;
                    this.RaisePropertyChanged(() => this.Message);
                }
            }
        }

        public System.Windows.Input.ICommand OKCommand
        {
            get { return _OKCommand; }
        }

        private void DoOKCommand()
        {
            System.Windows.Forms.MessageBox.Show("OK!");
        }
    }
}