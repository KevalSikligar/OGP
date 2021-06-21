using System;
using System.Collections.Generic;
using System.Text;

namespace OGP_Portal.Service.Dtos
{
   public class FDD_DTO
    {
        public List<PartnerFDD> partnerFDDs { get; set; }
        public List<FddDetail> fddDetails { get; set; }
        public List<TotalOGP> totalOGPs { get; set; }
    }

    public class PartnerFDD
    { 
        public long PartnerId { get; set; }
        public string BusinessPartner { get; set; }
        public int TotalPlanned { get; set; }
        public int Delivered { get; set; }
        public int Balance { get; set; }
    
    }

    public class FddDetail
    {
        public long PartnerId { get; set; }
        public int Projected { get; set; }
        public int Actual { get; set; }
        public string ForcastDate { get; set; }
        public int Total { get; set; }


    }


    public class TotalOGP
    {

        public string ForcastDate { get; set; }
        public int Projected { get; set; }
        public int Actual { get; set; }
    }
}
