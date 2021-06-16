using System;
using System.Collections.Generic;
using System.Text;

namespace OGP_Portal.Service.Dtos
{
  public  class OGPEntryDTO:BaseModel
    {

            public int G_ShellReceipt {get;set;}
            public int G_ShellWelding {get;set;}
            public int G_TopD_end_Receipt {get;set;}
            public int G_Fabrication {get;set;}
            public int G_Sent_to_paint {get;set;}
            public int G_Received_after_Paint {get;set;}
            public int G_Readiness {get;set;}
            public int G_BDE_Receipt {get;set;}
            public int G_BDE_readiness {get;set;}
            public int G_Shell_BDE {get;set;}
            public int G_Shell_BDE_Sent_to_Paint {get;set;}
            public int G_Shell_BDE_Received_after_paint {get;set;}
            public int G_SH_BDE_TDE {get;set;}
            public int G_Skid_Receipt {get;set;}
            public int G_Installation_Skid {get;set;}
            public int G_APT {get;set;}
            public int G_HPT {get;set;}
            public int G_Final_Paint {get;set;}
            public int G_Dispatch{get;set;}
            public int R_ShellReceipt{get;set;}
            public int R_ShellWelding{get;set;}
            public int R_TopD_end_Receipt{get;set;}
            public int R_Fabrication{get;set;}
            public int R_Sent_to_paint{get;set;}
            public int R_Received_after_Paint{get;set;}
            public int R_Readiness{get;set;}
            public int R_BDE_Receipt{get;set;}
            public int R_BDE_readiness{get;set;}
            public int R_Shell_BDE{get;set;}
            public int R_Shell_BDE_Sent_to_Paint{get;set;}
            public int R_Shell_BDE_Received_after_paint{get;set;}
            public int R_SH_BDE_TDE{get;set;}
            public int R_Skid_Receipt{get;set;}
            public int R_Installation_Skid{get;set;}
            public int R_APT{get;set;}
            public int R_HPT{get;set;}
            public int R_Final_Paint{get;set;}
            public int R_Dispatch { get; set; }
            public int MaxOGP { get; set; }
		public string PartnerName { get; set; }
		public int Balance { get; set; }
	}
}
