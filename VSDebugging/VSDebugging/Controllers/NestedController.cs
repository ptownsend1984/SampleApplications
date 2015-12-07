using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VSDebugging.Controllers
{
    public class NestedController
    {

        public void DoSomething()
        {
            this.HasHit7 = false;
            this.DoingSomething += this.OnDoingSomething;
            try
            {
                DoSomething1();
            }
            finally
            {
                this.DoingSomething -= this.DoingSomething;
            }
        }
        private void DoSomething1()
        {
            DoSomething2();
        }
        private void DoSomething2()
        {
            DoSomething3();
        }
        private void DoSomething3()
        {
            DoSomething4();
        }
        private void DoSomething4()
        {
            DoSomething5();
        }
        private void DoSomething5()
        {
            this.GetType().GetMethod("DoSomething6", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(this, null);
        }
        private void DoSomething6()
        {
            var Handler = this.DoingSomething;
            if (Handler != null)
                Handler(this, EventArgs.Empty);
        }
        private void DoSomething7()
        {
            HasHit7 = true;

            var Handler = this.DoingSomething;
            var Invocations = Handler.GetInvocationList();
            foreach (var Item in Invocations)
            {
                Item.DynamicInvoke(null, EventArgs.Empty);
            }
        }
        private void DoSomething8()
        {
            End();
        }
        private void End()
        {
            var StackFrame = new System.Diagnostics.StackFrame();
            var StackTrace = new System.Diagnostics.StackTrace();

            Console.WriteLine("Frames: {0}", StackTrace.FrameCount);
            var Path = string.Empty;
            var TraceFrames = StackTrace.GetFrames().Select(o => o.ToString());

            Console.WriteLine("Methods called:\r\n{0}", string.Join("-->\r\n", TraceFrames));
        }

        private bool HasHit7 = false;
        public event EventHandler DoingSomething;
        private void OnDoingSomething(object sender, EventArgs e)
        {
            if (!HasHit7)
                DoSomething7();
            else
                DoSomething8();
        }

    }
}