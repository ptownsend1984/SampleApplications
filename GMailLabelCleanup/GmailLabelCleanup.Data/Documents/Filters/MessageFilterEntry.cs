using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GMailLabelCleanup.Data.Documents.Filters
{
    public class MessageFilterEntry
    {

        public string IdTag { get; set; }

        public IList<MessageFilterProperty> Properties { get; set; }

    }
}