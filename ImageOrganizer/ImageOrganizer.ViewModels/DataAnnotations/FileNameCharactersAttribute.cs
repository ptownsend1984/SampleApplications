using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ImageOrganizer.ViewModels.DataAnnotations
{
    public class FileNameCharactersAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            var Value = Convert.ToString(value);
            if (string.IsNullOrEmpty(Value))
                return true; //Not invalid here if empty.  Should be caught by a RequiredAttribute instead
            else
            {
                var InvalidPathChars = System.IO.Path.GetInvalidFileNameChars();
                return !Value.Any((c) => InvalidPathChars.Contains(c));
            }
        }

    }
}