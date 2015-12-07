using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MEFContracts;

namespace MEFPresentation
{
    [Export]
    public class PresentationModule
    {

        #region Global Variables

        private readonly CompositionContainer Container;
        private readonly System.Timers.Timer Timer;        

        #endregion

        #region Constructor

        [ImportingConstructor]
        public PresentationModule(CompositionContainer Container)
        {
            this.Container = Container;
            Timer = new System.Timers.Timer(1500);
            Timer.AutoReset = true;
            Timer.Elapsed += Timer_Click;
        }

        #endregion

        #region Event Handlers

        private void Timer_Click(object sender, EventArgs e)
        {
            var MainViewModel = this.Container.GetExportedValue<IMainViewModel>();
            if (MainViewModel != null)
            {
                MainViewModel.Message = DateTime.Now.ToString();
            }
        }

        #endregion

        #region Methods

        public void StartTimer()
        {
            Timer.Start();
        }

        #endregion

    }
}