using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace WebServices.Models
{
    [DataContract]
    public class Pickle
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Bumps { get; set; }

    }
}