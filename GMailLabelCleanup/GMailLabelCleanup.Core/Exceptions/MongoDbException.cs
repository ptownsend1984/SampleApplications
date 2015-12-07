using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using MongoDB.Driver;

namespace GMailLabelCleanup.Core.Exceptions
{
    public class MongoDbException : Exception
    {

        public GetLastErrorResult Result { get; private set; }

        public MongoDbException(string message, GetLastErrorResult result)
            : base(message)
        {
            if (result == null)
                throw new ArgumentNullException("result");
            
            this.Result = result;
        }
        public MongoDbException(string message, GetLastErrorResult result, Exception innerException)
            : base(message, innerException)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            this.Result = result;
        }

    }
}