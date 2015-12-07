using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VSDebugging.Models
{
    [System.Diagnostics.DebuggerDisplay("ID: {ID}")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(EmployeeProxyView))]
    [System.Diagnostics.DebuggerVisualizer(typeof(EmployeeVisualizer))]
    [Serializable]
    public class Employee
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

    }
}