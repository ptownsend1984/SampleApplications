using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.DebuggerVisualizers;
using VSDebugging.Forms;

namespace VSDebugging.Models
{
    public class EmployeeVisualizer : DialogDebuggerVisualizer
    {

        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var Model = objectProvider.GetObject() as Employee;
            if (Model == null)
                throw new InvalidCastException("Object is not an employee.");

            var Dialog = new EmployeeVisualizerForm();
            Dialog.Model = Model;

            try
            {
                windowService.ShowDialog(Dialog);
            }
            finally
            {
                Dialog.Model = null;
            }
        }
    }

}