using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace GMailLabelCleanup.Core.ViewModels.Account
{
    public class ExternalLoginConfirmationViewModel
    {

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

    }
}