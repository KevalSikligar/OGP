using OGP_Portal.Data.DbContext;
using OGP_Portal.Data.DbModel.BaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OGP_Portal.Data.DbModel
{
    public class Forcast : EntityWithAudit
    {
		public DateTime ForcastDate { get; set; }

		public int  Quantity { get; set; }
        [Required]
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
