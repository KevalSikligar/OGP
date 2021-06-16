using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunTechnology.Utility
{
    public static class GlobalConstant
    {
        /* Default Project Name*/
        public const string DefaultProjectName = "JunTechnology";

        public const string AlreadyRegisterd = "The email you have entered, is already exists.";
        public const string NotRegistered = "User not created, Please try again !";
        public const string RegisterSuccessfully = "Registration done successfully.";
        public const string RegisterEmailNotSent = "User is successfully created but we failed to sent email. Please contact administrator for help.";
        public const string InvalidLogin = "Login Failed. Invalid username or password.";
        public const string ConfirmEmail = "Please confirm your account.";
        public const string AccountClosed = "Your account is closed.";
        public const string AccountDeactivated = "Your account is deactivated.";
        public const string LoginSuccessfully = "Logged in successfully.";
        public const string AlreadyLogin = "Already logged in.";
        public const string EmailNotFound = "User not found !";
        public const string EmailConfirm = "Email confirmed successfully.";
        public const string EmailResetPassword = "Please check your Email to reset your password !";
        public const string PasswordChanged = "Password changed successfully.";
        public const string PasswordNotChanged = "Password has not been changed, Please try again later !";
        public const string LogoutSuccessfully = "Logged out successfully.";
        public const string UserUpdatedSuccessfully = "User updated successfully.";
        public const string RoleListSuccessfully = "Get role list successfully.";
        public const string SomethingWrong = "Something went wrong !";
        public const string CompanyNotApproved = "Your account is not approved by admin.";

        public const string UnhandledError = "Unhandled error occured. Please try again later !";
        public const string InvalidModel = "Invalid Details.";
        public const string InvalidLink = "Link you tried to access is invalid.";
        public const string LinkExpired = "Link you tried to access is expired.";

        public const string WrongCode = "You have entered wrong code.";

        //---- Code Already exist ----
        public const string AlreadyExistCode = "Code already exist. Please try another code.";

        //---- Name Already exist ----
        public const string AlreadyExistName = "Name already exist. Please try another Name.";

        /*: Question set is already exist*/
        public const string AlreadyExistId = "Question set already exist. Please try another Id.";

        public const string DetailNotFound = "Detail not found !";

        /*Delete Recored*/
        public const string DeleteRecord = "Record deleted successfully.";

        /* Invalid current password*/
        public const string InvalidCurrentPassword = "Invalid current password !";
        public const string UserNotFound = "User not found.";

        /**Inactive User**/
        public const string DeActiveUser = "Unfortunately your password cannot be reset while your email is disabled. Please contact your Client Admin or System Admin.";



        /*Ticket */
        public const string NewTicket = "NewTicket";
        public const string Priority = "--Select Priority--";
        public const string Category = "--Select Category--";
        public const string Employee = "--Select Employee--";
        public const string User = "--Select User--";


        //Company
        public const string CreateNewCompany = "Company Created SuccessFully";
        public const string UpdateCompany = "Company Updated SuccessFully";
        public const string UpdateEmployee = "Employee Updated SuccessFully";
        public const string DeleteCompany = "Company Deleted SuccessFully";
        public const string DeleteEmployee = "Employee Deleted SuccessFully";
        public const string CreateEmployee = "Employee Created SuccessFully";

        //Category
        public const string CreateNewCategory = "Category Created SuccessFully";
        public const string UpdateCategory = "Category Updated SuccessFully";
        public const string DeleteCategory = "Category Deleted SuccessFully";

        //Department
        public const string CreateNewDepartment = "Department Created SuccessFully";
        public const string UpdateDepartment = "Department Updated SuccessFully";
        public const string DeleteDepartment = "Department Deleted SuccessFully";

        //Employee
        public const string EmployeeAssigned = "Employee Assigned SuccessFully";

        //Status
        public const string StatusChanged = "Status Changed SuccessFully";

        // ViewTicket

        public const string UserRedirection = "/Ticket/ViewTicket";
        public const string EmployeeRedirection = "/Company/Master/ViewTicket";
        public const string HrRedirection = "/Company/Master/ViewTicket";
        public const string AdminRedirection = "/Admin/Master/ViewTicket";



        public const string UserImage = @"\wwwroot\JunTechfrontend\img\userprofile.png";

    }

    public static class UserClaims
    {
        public const string UserId = "UserId";
        public const string EmployeeId = "EmployeeId";
        public const string UserRole = "UserRole";
        public const string FullName = "FullName";

    }
}
    
