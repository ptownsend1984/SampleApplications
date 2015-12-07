using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WPFDemo.ViewModels.Windows
{
    public class MainWindowViewModel : UIViewModel
    {

        #region Global Variables

        private decimal _CurrencyValue;
        private DateTime _DateValue;

        private readonly ObservableCollection<SelectableItem> _SelectableItemCollection;

        private DelegateCommand _ExitCommand;        

        #endregion

        #region Properties

        public IList<SelectableItem> SelectableItemCollection { get { return _SelectableItemCollection; } }

        public decimal CurrencyValue
        {
            get { return _CurrencyValue; }
            set
            {
                if (_CurrencyValue == value) return;
                _CurrencyValue = value;
                OnPropertyChanged(() => this.CurrencyValue);
            }
        }
        public DateTime DateValue
        {
            get { return _DateValue; }
            set
            {
                if (_DateValue == value) return;
                _DateValue = value;
                OnPropertyChanged(() => this.DateValue);
            }
        }

        public ICommand ExitCommand { get { return _ExitCommand; } }

        #endregion

        #region Events


        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            _SelectableItemCollection = new ObservableCollection<SelectableItem>();
            _SelectableItemCollection.Add(new SelectableItem() { IsSelected = false, Name = "One" });
            _SelectableItemCollection.Add(new SelectableItem() { IsSelected = true, Name = "Two" });
            _SelectableItemCollection.Add(new SelectableItem() { IsSelected = false, Name = "Three" });

            _DateValue = DateTime.Now;

            _ExitCommand = new DelegateCommand(DoExit, CanDoExit);
        }

        #endregion

        #region Event Handlers


        #endregion

        #region Methods

        private void DoExit()
        {
            if (!CanDoExit())
                return;
            this.OnRequestClose(true);
        }
        private bool CanDoExit()
        {
            return true;
        }

        #endregion

    }

    public class SelectableItem : ViewModel
    {
        private bool _IsSelected;
        private string _Name;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected == value) return;
                _IsSelected = value;
                OnPropertyChanged(() => this.IsSelected);
            }
        }
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value) return;
                _Name = value;
                OnPropertyChanged(() => this.Name);
            }
        }
    }
}