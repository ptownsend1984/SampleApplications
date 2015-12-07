using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSDebugging.Controllers;
using VSDebugging.Forms;

namespace VSDebugging
{

    class Program
    {
        static void Main(string[] args)
        {            
            var BreakpointController = new BreakpointController();
            BreakpointController.Breakpoints();
            BreakpointController.TestDebuggerAttributes();

            var NestingController = new NestedController();
            NestingController.DoSomething();

            var ModelController = new ModelController();
            ModelController.CreateCompany();
            ModelController.CreateEmployee();
            ModelController.ShowBuggy();

            var FormsController = new FormsController();
            FormsController.ShowCrossThreadForm();
        }

    }
}































/*
 * 0: Breakpoints
 *      Placement
 *      Switch to Frame
 *      Conditional/Hit Count
 *      DataTips
 *      Debugger.Break
 *      Step In/Over/Out, Run To Cursor, Drag executing arrow
 *      Edit&Continue
 *      StackTrace
 *      JustMyCode
 * 1: Watches
 *      Auto
 *      Locals
 *      Quickwatch
 * 2: Immediate Window
 *      Get/Set values
 * 3: Multithreading
 * 4: Attach to process (IIS)
 * 5: Debugging Attributes
 *      DebuggerStepThrough
 *      DebuggerDisplay
 * 9: Gotchas
 *      Out of date file/DLL
 *      Function eval timed out (Cross-thread/deadlock)
*/
