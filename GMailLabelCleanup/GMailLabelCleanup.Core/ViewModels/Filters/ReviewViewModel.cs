using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GMailLabelCleanup.Data.Documents.Filters;
using System.ComponentModel.DataAnnotations;

namespace GMailLabelCleanup.Core.ViewModels.Filters
{
    public class ReviewViewModel
    {
        public bool SelectAll { get; set; }

        public string MessageFilterId { get; set; }
        public IList<ChooseMessageFilterEntryViewModel> Entries { get; set; }

    }

    public class ChooseMessageFilterEntryViewModel
    {
        
        public bool IsSelected { get; set; }

        public string EntryId { get; set; }
        public IList<MessageFilterProperty> CriteriaProperties { get; set; }
        public IList<MessageFilterProperty> ActionProperties { get; set; }

    }
}