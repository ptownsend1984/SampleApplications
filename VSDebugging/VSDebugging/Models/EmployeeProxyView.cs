using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VSDebugging.Models
{
    public class EmployeeProxyView
    {

        private readonly Employee _Employee;
        public const string HELLO = "HELLO WORLD";

        public EmployeeProxyView(Employee model)
        {
            this._Employee = model;
        }

    }
}