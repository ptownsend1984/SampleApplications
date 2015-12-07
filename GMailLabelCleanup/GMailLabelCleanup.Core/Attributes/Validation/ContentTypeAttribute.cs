using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using GMailLabelCleanup.Common.Extensions;

namespace GMailLabelCleanup.Core.Attributes.Validation
{
    public class ContentTypeAttribute : ValidationAttribute
    {

        private readonly string _allowedContentTypes;

        public ContentTypeAttribute(string allowedContentTypes)
        {
            _allowedContentTypes = allowedContentTypes;
        }

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file == null)
                return true;
            
            var types = _allowedContentTypes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(o => o.TrimIfNotEmpty());
            return types.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase);
        }
        public override string FormatErrorMessage(string name)
        {
            return "Content type not allowed.";
        }

    }
}