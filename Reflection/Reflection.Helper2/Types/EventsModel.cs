using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Reflection.Helper2.Types
{
    public class EventsModel
    {
        public int Value { get; set; }

        public event EventHandler Promoted;

        public void RaisePromoted()
        {
            var handler = Promoted;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

    }
}