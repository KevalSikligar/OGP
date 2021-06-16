using OGP_Portal.Data.DbContext;
using OGP_Portal.Data.DbModel.BaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OGP_Portal.Data.DbModel
{
    public class Entry:EntityWithAudit
    {
        public string VendorName { get; set; }
        public int AsslyType { get; set; }
        public int ShellReceipt { get; set; }
        public int ShellWelding { get; set; }
        public int TopD_end_Receipt { get; set; }
        public int Fabrication { get; set; }
        public int Sent_to_paint { get; set; }
        public int Received_after_Paint { get; set; }
        public int Readiness { get; set; }
        public int BDE_Receipt { get; set; }
        public int BDE_readiness { get; set; }
        public int Shell_BDE { get; set; }
        public int Shell_BDE_Sent_to_Paint { get; set; }
        public int Shell_BDE_Received_after_paint { get; set; }
        public int SH_BDE_TDE { get; set; }
        public int Skid_Receipt { get; set; }
        public int Installation_Skid { get; set; }
        public int APT { get; set; }
        public int HPT { get; set; }
        public int Final_Paint { get; set; }
        public int Dispatch { get; set; }

        [Required]
        public long PartnerId { get; set; }
        [ForeignKey("PartnerId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
