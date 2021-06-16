using System;
using System.Collections.Generic;
using System.Text;

namespace OGP_Portal.Service.Dtos
{
    public class EntryDto : BaseModel
    {
        public string VendorName { get; set; }
        public string Name { get; set; }
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

        public long PartnerId { get; set; }
        public int OGP_Limit { get; set; }
        public long EntryId { get; set; }
        public string CreatedDateSTR { get; set; }
        public string asslytypeName { get; set; }
        public bool isOnlyEntry { get; set; }
    }


    public class BUserEntryListDto
    {
        public string CreatedDateSTR { get; set; }
       
        public string username { get; set; }
        public string Name { get; set; }
        public string asslytype { get; set; }
        public string asslytypeName { get; set; }
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
        public long PartnerId { get; set; }
        public long TotalRecords { get; set; }
        public long Id { get; set; }

    }

    public class UserNameList {

        public string UserName { get; set; }
    }
}
