using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GMailLabelCleanup.Data.Models.Filters;

namespace GMailLabelCleanup.Core.ViewModels.Filters
{
    public class IndexViewModel
    {

        public IList<Filter> Filters { get; set; }

    }
}