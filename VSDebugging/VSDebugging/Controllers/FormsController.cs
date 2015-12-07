using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VSDebugging.Forms;

namespace VSDebugging.Controllers
{
    public class FormsController
    {

        public void ShowCrossThreadForm()
        {
            var Form = new CrossThreadForm();
            Form.ShowDialog();
        }


    }
}