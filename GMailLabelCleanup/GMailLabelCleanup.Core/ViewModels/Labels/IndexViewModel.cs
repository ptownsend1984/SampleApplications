using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GMailLabelCleanup.Core.Models;

namespace GMailLabelCleanup.Core.ViewModels.Labels
{
    public class IndexViewModel
    {

        public IReadOnlyList<LabelInfo> Labels { get; set; }

    }
}