using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GMailLabelCleanup.Core.ViewModels.Filters
{
    public class EditViewModel
    {

        public int Id { get; set; }
        public byte[] Timestamp { get; set; }

        [MaxLength(500)]
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public IList<EditPropertyViewModel> CriteriaProperties { get; set; }
        public IList<EditPropertyViewModel> ActionProperties { get; set; }

        [ScaffoldColumn(false)]
        public SelectList SmartLabels { get; set; }
        [ScaffoldColumn(false)]
        public SelectList SizeOperators { get; set; }
        [ScaffoldColumn(false)]
        public SelectList SizeUnits { get; set; }

    }

    public class EditPropertyViewModel
    {

        public int? Id { get; set; }
        public byte[] Timestamp { get; set; }

        public bool IsIncluded { get; set; }

        [MaxLength(256)]
        [Required]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Value { get; set; }

        public bool IsCheckType { get; set; }
        public bool IsChecked { get; set; }

        public bool IsSelectType { get; set; }
        public int SelectionId { get; set; }

        [ScaffoldColumn(false)]
        public SelectList Selections { get; set; }

    }
}