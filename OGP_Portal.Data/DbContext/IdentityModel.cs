using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OGP_Portal.Data.DbContext
{
    public class UserRole : IdentityUserRole<long> { }

    public class UserClaim : IdentityUserClaim<long> { }

    public class UserLogin : IdentityUserLogin<long> { }

    public class Role : IdentityRole<long>
    {
        public Role()
        {
            DisplayRoleName = "";
        }
        [NotMapped]
        public string DisplayRoleName { get; set; }

    }

    public class ApplicationUser : IdentityUser<long>
    {


        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }


        [StringLength(50)]
        public string LastName { get; set; }

        [NotMapped] public string FullName => $@"{FirstName} {LastName}";


        [Required, StringLength(10)]
        public string MobileNumber { get; set; }

       

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [DefaultValue(false)]
        public bool IsPasssReset { get; set; }


    }
}
