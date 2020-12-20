using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace STEC.Services.UserTenants
{
    public class User : IdentityUser
    {
        [PersonalData]
        [Display(Name = "First name")]
        public string FirstName { get; set; }


        [PersonalData]
        [Display(Name = "Last name")]
        public string LastName { get; set; }


        public UserRole Role { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
