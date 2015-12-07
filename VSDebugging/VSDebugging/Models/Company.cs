using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VSDebugging.Models
{
    [System.Diagnostics.DebuggerDisplay("ID: {ID} / Name: {Name} / Count: {PhoneNumbers.Count}")]
    public class Company
    {

        public int ID { get; set; }
        public string Name { get; set; }

        private string _Location;
        public string Location
        {
            get { return _Location; }
            set { _Location = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private List<string> _PhoneNumbers;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
        public List<string> PhoneNumbers
        {
            get { return _PhoneNumbers; }
        }

        //[System.Diagnostics.DebuggerStepThrough]
        public Company()
        {
            _PhoneNumbers = new List<string>();
        }

    }
}