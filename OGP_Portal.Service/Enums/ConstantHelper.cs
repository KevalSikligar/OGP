using System;
using System.Collections.Generic;
using System.Text;

namespace OGP_Portal.Service.Enums
{
   
    public class UserRoles
        {
            public const string Administrator = "Administrator";
            public const string BusinessPartner = "BusinessPartner";
            public const string BusinessUser = "BusinessUser";
         
         
        }
    
    public class EmailTemplateList
    {
        public const string EmailTemplate = @"EmailTemplate\";
    }
    public class EmailTemplateFileList
    {
        public const string ResetPassword = @"ResetPassword.html";
        public const string Confirmation = @"ConfirmEmail.html";
        public const string ConfirmEmailEmployee = @"ConfirmEmailEmployee.html";
        public const string CreateTicket = @"CreateTicket.html";

    }
    public class StoredProcedureList
    {
        public const string GetB_UsersList = @"GetB_UsersList";
        public const string GetB_PartnerList = @"GetB_PartnerList";
        public const string GetBalance = @"GetBalance";
        public const string GetEntryList = @"GetEntryList";
        public const string GetForcastList = @"GetForcastList";
        public const string GetBusinessPartnerEntryList = @"GetBusinessPartnerEntryList";
        public const string GetLogEntryList = @"GetLogEntryList";
        public const string GetAdminDashboardEntryList = @"GetAdminDashboardEntryList";
        public const string GetNewEntryList = @"GetNewEntryList";
        public const string GetUserSide_B_HistoryList = @"GetUserSide_B_HistoryList";
    }


}

