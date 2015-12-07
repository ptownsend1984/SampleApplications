using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNet.Identity.EntityFramework;
using GMailLabelCleanup.Data.Models.Identity;
using GMailLabelCleanup.Data.Models.Filters;
using System.Data.Entity;

namespace GMailLabelCleanup.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public IDbSet<Filter> Filters { get; set; }
        public IDbSet<FilterProperty> FilterProperties { get; set; }

        public IDbSet<GoogleAuthData> GoogleAuthData { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}