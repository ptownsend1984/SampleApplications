using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MongoDB.Bson;

namespace GMailLabelCleanup.Data.Documents.Filters
{
    public class MessageFilter : Document
    {
        public IList<MessageFilterEntry> Entries { get; set; }

    }
}