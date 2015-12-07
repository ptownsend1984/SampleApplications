using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace CustomSnapIns
{
    [System.ComponentModel.RunInstaller(true)]
    public class ThreadingPSSnapIn : PSSnapIn
    {

        public override string Description
        {
            get { return "This snap in will help make things easier with threads."; }
        }

        public override string Name
        {
            get { return "Threading"; }
        }

        public override string Vendor
        {
            get { return "Peter Townsend"; }
        }
    }
}