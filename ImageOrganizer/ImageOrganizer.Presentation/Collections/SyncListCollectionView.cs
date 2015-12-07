using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Threading;

namespace ImageOrganizer.Presentation.Collections
{
    public class SyncListCollectionView : ListCollectionView
    {

        #region Global Variables


        #endregion

        #region Constructor

        public SyncListCollectionView(IList List)
            : base(List)
        {            
        }

        #endregion

        #region Methods

        public override bool MoveCurrentToPosition(int position)
        {
            return (bool)this.Dispatcher.Invoke(new Func<int, bool>(base.MoveCurrentToPosition), position);
        }
        public override void Refresh()
        {
            this.Dispatcher.Invoke(new Action(base.Refresh));
        }

        #endregion

    }
}