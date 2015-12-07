using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace GMailLabelCleanup.Core.Attributes.Validation
{
    public class FileSizeAttribute : ValidationAttribute
    {

        private readonly int _maxFileSize;

        public FileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = Math.Max(0, maxFileSize);
        }

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file == null)
                return true;

            return file.ContentLength <= _maxFileSize;            
        }
        public override string FormatErrorMessage(string name)
        {
            return string.Format("The file size cannot exceed {0}.", GetFriendlyFileSize(_maxFileSize));
        }
        private string GetFriendlyFileSize(int value)
        {
            if (value > (1 << 20))
            {
                return (value / (1 << 20)).ToString() + " MB";
            }
            else if (value > (1 << 10))
            {
                return (value / (1 << 10)).ToString() + " KB";
            }
            else
            {
                return value.ToString() + " bytes";
            }
        }

    }
}