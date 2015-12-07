using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VSDebugging.Models
{
    public class Buggy
    {
        private System.Threading.Semaphore Signal = new System.Threading.Semaphore(1, 1);

        public int Value
        {
            get
            {
                if (System.Diagnostics.Debugger.IsAttached)
                    Console.WriteLine("Value");
                Signal.WaitOne();                
                return 0;
            }
        }


    }
}