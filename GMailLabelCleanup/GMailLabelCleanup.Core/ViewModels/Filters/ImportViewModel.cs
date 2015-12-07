using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using GMailLabelCleanup.Core.Attributes.Validation;

namespace GMailLabelCleanup.Core.ViewModels.Filters
{
    public class ImportViewModel
    {

        [Display(Name = "Choose a file")]
        [Required]
        [DataType(DataType.Upload)]
        [FileSize(1 << 20)]
        [ContentType("text/xml")]
        public HttpPostedFileBase UploadFile { get; set; }

    }
}