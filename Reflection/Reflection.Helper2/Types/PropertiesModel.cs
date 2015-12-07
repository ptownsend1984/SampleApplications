using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Reflection.Helper2.Types
{
    public class PropertiesModel
    {

        private int _x = 5;
        private int _z;

        public int X { get { return _x; } }

        public int Y { get; set; }

        public int Z { set { _z = value; } }

        public int W { get; private set; }

        public string Elephant { get { return "Elephant"; } }

        public int this[int x]
        {
            get { return _x + x; }
        }        

        public int GetZ() { return _z; }

    }
}