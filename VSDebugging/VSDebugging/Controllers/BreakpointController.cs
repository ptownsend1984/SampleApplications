using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VSDebugging.Controllers
{
    public class BreakpointController
    {

        public void Breakpoints()
        {
            int x;
            float y;

            x = 0;
            y = 0f;

            var z = 0d;

            for (int i = 0; i < 50; i++)
            {
                x++;
                y += (float)new Random().NextDouble() * (float)x;
                z -= y;
            }
        }


        public void TestDebuggerAttributes()
        {
            StepThrough();
        }
        [System.Diagnostics.DebuggerStepThrough]
        private int StepThrough()
        {
            Console.Write("BreakpointController.StepThrough");
            return new Random().Next();
        }


    }
}