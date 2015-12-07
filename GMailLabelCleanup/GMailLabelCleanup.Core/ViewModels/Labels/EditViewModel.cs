using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GMailLabelCleanup.Core.Models;

namespace GMailLabelCleanup.Core.ViewModels.Labels
{
    public class EditViewModel
    {

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public LabelType LabelType { get; set; }

        [Required]
        public int LabelListVisibilityType { get; set; }
        [Required]
        public int MessageListVisibilityType { get; set; }

        [ScaffoldColumn(false)]
        public SelectList LabelListVisibilityTypes { get; set; }
        [ScaffoldColumn(false)]
        public SelectList MessageListVisibilityTypes { get; set; }

    }
}