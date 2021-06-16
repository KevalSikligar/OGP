using OGP_Portal.Data.DbContext;
using OGP_Portal.Data.DbModel.BaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OGP_Portal.Data.DbModel
{
   public class B_Partner:EntityWithAudit
    {

        [Required]
        public string Location { get; set; }

        [Required]
        public int Quantity_of_OGP { get; set; }

        [Required]
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
