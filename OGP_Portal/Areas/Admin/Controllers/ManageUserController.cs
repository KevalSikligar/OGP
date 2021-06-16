using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OGP_Portal.Common;
using OGP_Portal.Data.DbContext;
using OGP_Portal.Data.DbModel;
using OGP_Portal.Models;
using OGP_Portal.Service.Dtos;
using OGP_Portal.Service.Enums;
using OGP_Portal.Service.Exceptions;
using OGP_Portal.Service.Interface;
using OGP_Portal.Service.Utility;
using OGP_Portal.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.Extensions.Options;

namespace OGP_Portal.Areas.Admin.Controllers
{
    [Authorize(Roles = UserRoles.Administrator), Area("Admin")]
    public class ManageUserController : BaseController<ManageUserController>
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IB_UserService _b_userService;
        private readonly IB_PartnerService _b_partner;
        private readonly EmailService _emailService;
        private readonly IErrorLogService _errorLog;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private Microsoft.Extensions.Configuration.IConfiguration _config;
        #endregion

        #region Ctor
        public ManageUserController(UserManager<ApplicationUser> userManager, 
            IUserService userService, IB_UserService b_UserService, IB_PartnerService b_PartnerService,
            IErrorLogService errorLog,
            Microsoft.Extensions.Configuration.IConfiguration config, IOptions<EmailSettingsGmail> emailSettingsGmail, IWebHostEnvironment webHostEnvironment
            )
        {
            _userManager = userManager;
            _userService = userService;
            _b_userService = b_UserService;
            _b_partner = b_PartnerService;
            _emailService = new EmailService(emailSettingsGmail);
            _webHostEnvironment = webHostEnvironment;
            _errorLog = errorLog;
            _config = config;
        }
        #endregion

        #region Methods

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Get_BUser()
        {

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> AddEditBUser(long id)
        {
            if (id == 0) return base.View(@"Components/_AddEditBUser", new BUserDto { Id = id });
            var result = await _userManager.FindByIdAsync(id.ToString());
            var tempView = new BUserDto();
            tempView.Name = result.FirstName;
            tempView.Email = result.Email;
            tempView.Id = result.Id;
            tempView.Mobile = result.MobileNumber;
            
            return View(@"Components/_AddEditBUser", tempView);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditBUser(BUserDto model)
        {
            using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        txscope.Dispose();
                        return RedirectToAction("AddEditBUser", model.Id);
                    }

                    var isExist =  _userService.GetSingle(x => x.Email.ToLower().Equals(model.Email.ToLower()) && x.IsActive);
                    if (isExist==null)
                    {

                        ApplicationUser user = new ApplicationUser();
                        user.UserName = model.Email;
                        user.FirstName = model.Name;
                        user.Email = model.Email;
                        user.IsActive = true;
                        user.MobileNumber = model.Mobile;
                        user.IsActive = true;
                        var userResult = await _userManager.CreateAsync(user, model.Password);

                        if (userResult.Succeeded)
                        {
                            B_User bUser = new B_User();
                            bUser.UserId = user.Id;
                            bUser.IsActive = true;
                            var bUserResult = await _b_userService.InsertAsync(bUser, Accessor, User.GetUserId());
                            if (bUserResult != null)
                            {
                                var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.BusinessUser);
                                user.EmailConfirmed = true;
                                var EmailResult = await _userManager.UpdateAsync(user);
                                if (EmailResult!=null)
                                {

                                   // SendEmail(user, "OGP_Portal", model.Password);
                                    txscope.Complete();
                                    return JsonResponse.GenerateJsonResult(1, "User  Created");
                                }
                                else
                                {
                                    txscope.Dispose();
                                    return JsonResponse.GenerateJsonResult(0, "something went wrong");
                                }
                            }
                            else
                            {
                                txscope.Dispose();
                                return JsonResponse.GenerateJsonResult(0, "Something went wrong");
                            }
                        }
                        else { 
                                txscope.Dispose();
                                return JsonResponse.GenerateJsonResult(0, "Something went wrong.");
                             }

                    }
                    else
                    {

                        var user = await _userManager.FindByIdAsync(model.Id.ToString());
                        user.FirstName = model.Name;
                        user.MobileNumber = model.Mobile;
                        if (model.Password != "" && model.Password!= null)
                        {
                            await _userManager.RemovePasswordAsync(user);
                            await _userManager.AddPasswordAsync(user, model.Password);
                        }

                        if (model.Email != "" && user.Email != model.Email)
                        {
                            user.Email = model.Email;
                            user.NormalizedEmail = model.Email.ToUpper();
                            user.UserName = model.Email;
                        }
                        var updateResult = await _userManager.UpdateAsync(user);
                        if (updateResult.Succeeded)
                        {
                            txscope.Complete();
                            return JsonResponse.GenerateJsonResult(1, "User Updated");
                        }
                        else { 
                            txscope.Dispose();
                            return JsonResponse.GenerateJsonResult(0, "something went wrong");
                        
                        }

                    }

                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "CreateCompany");
                    return JsonResponse.GenerateJsonResult(0, "");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBUserList(JQueryDataTableParamModel param)
        {
            using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;

                    var allList = await _userService.GetBUserList(parameters.ToArray());


                    var total = allList.FirstOrDefault()?.TotalRecords ?? 0;
                    return Json(new
                    {
                        param.sEcho,
                        iTotalRecords = total,
                        iTotalDisplayRecords = total,
                        aaData = allList
                    });
                }
                catch (Exception ex)
                {
                    ErrorLog.AddErrorLog(ex, "GetBUserList");
                    return Json(new
                    {
                        param.sEcho,
                        iTotalRecords = 0,
                        iTotalDisplayRecords = 0,
                        aaData = ""
                    });
                }
            }
        }


        public IActionResult Get_BPartner()
        {

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> _AddEditBPartner(long id)
        {
            if (id == 0) return base.View(@"Components/_AddEditBPartner", new BPartnerDto { Id = id });
            
            var result = await _userManager.FindByIdAsync(id.ToString());
            var tempView = new BPartnerDto();
            tempView.Name = result.FirstName;
            tempView.Email = result.Email;
            tempView.Id = result.Id;
            tempView.Mobile = result.MobileNumber;
            tempView.Quantity_of_OGP = _b_partner.GetSingle(x=>x.UserId==result.Id).Quantity_of_OGP;
            tempView.location = _b_partner.GetSingle(x=>x.UserId==result.Id).Location;
            
            
          
            return View(@"Components/_AddEditBPartner", tempView);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditBPartner(BPartnerDto model)
        {
            using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        txscope.Dispose();
                        return RedirectToAction("_AddEditBPartner", model.Id);
                    }
                    var isExist = _userService.GetSingle(x => x.Email.ToLower().Equals(model.Email.ToLower()) && x.IsActive);
                    if (isExist == null)
                    {

                        ApplicationUser user = new ApplicationUser();
                        user.UserName = model.Email;
                        user.FirstName = model.Name;
                        user.Email = model.Email;
                        user.IsActive = true;
                        user.MobileNumber = model.Mobile;
                        user.IsActive = true;
                        var userResult = await _userManager.CreateAsync(user, model.Password);

                        if (userResult.Succeeded)
                        {
                            B_Partner bUser = new B_Partner();
                            bUser.UserId = user.Id;
                            bUser.IsActive = true;
                            bUser.Quantity_of_OGP = model.Quantity_of_OGP;
                            bUser.Location = model.location;
                            var bUserResult = await _b_partner.InsertAsync(bUser, Accessor, User.GetUserId());
                            if (bUserResult != null)
                            {
                                var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.BusinessPartner);
                                if (roleResult.Succeeded)
                                {
                                    SendEmail(user, "OGP_Portal", model.Password);
                                    txscope.Complete();
                                    return JsonResponse.GenerateJsonResult(1, "User  Created");
                                }
                                else
                                {
                                    txscope.Dispose();
                                    return JsonResponse.GenerateJsonResult(0, "something went wrong");
                                }
                            }
                            else
                            {
                                txscope.Dispose();
                                return JsonResponse.GenerateJsonResult(0, "Something went wrong");
                            }
                        }
                        else
                        {
                            txscope.Dispose();
                            return JsonResponse.GenerateJsonResult(0, "Something went wrong.");
                        }

                    }
                    else
                    {

                        var user = await _userManager.FindByIdAsync(model.Id.ToString());
                        user.FirstName = model.Name;
                        user.MobileNumber = model.Mobile;
                        if (model.Password != "")
                        {
                            await _userManager.RemovePasswordAsync(user);
                            await _userManager.AddPasswordAsync(user, model.Password);
                        }

                        if (model.Email != "" && user.Email != model.Email)
                        {
                            user.Email = model.Email;
                            user.NormalizedEmail = model.Email.ToUpper();
                            user.UserName = model.Email;
                        }

                        var updateResult = await _userManager.UpdateAsync(user);
                        if (updateResult.Succeeded)
                        {

                            var partner = _b_partner.GetSingle(x=>x.UserId==user.Id);
                            partner.Quantity_of_OGP = model.Quantity_of_OGP;
                            var pUpdateResult = await _b_partner.UpdateAsync(partner, Accessor, User.GetUserId());
                            txscope.Complete();
                            return JsonResponse.GenerateJsonResult(1, "Partner Updated");
                        }
                        else
                        {
                            txscope.Dispose();
                            return JsonResponse.GenerateJsonResult(0, "something went wrong");

                        }

                    }

                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "CreateCompany");
                    return JsonResponse.GenerateJsonResult(0, "");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBPartnerList(JQueryDataTableParamModel param)
        {
            using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;

                    var allList = await _userService.GetBPartnerList(parameters.ToArray());


                    var total = allList.FirstOrDefault()?.TotalRecords ?? 0;
                    return Json(new
                    {
                        param.sEcho,
                        iTotalRecords = total,
                        iTotalDisplayRecords = total,
                        aaData = allList
                    });
                }
                catch (Exception ex)
                {
                    ErrorLog.AddErrorLog(ex, "GetBPartnerList");
                    return Json(new
                    {
                        param.sEcho,
                        iTotalRecords = 0,
                        iTotalDisplayRecords = 0,
                        aaData = ""
                    });
                }
            }
        }


        [HttpGet]
        public async Task<IActionResult> RemoveUSer(long id)
        {
            using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var questionObj = _userService.GetSingle(x => x.Id == id);
                    questionObj.IsActive = false;
                    questionObj.EmailConfirmed = false;
                    await _userService.UpdateAsync(questionObj, Accessor,User.GetUserId());
                    txscope.Complete();

                    return JsonResponse.GenerateJsonResult(1, "User removed");
                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "Post-RemoveUSer");
                    return JsonResponse.GenerateJsonResult(0, "");
                }
            }
        }


        [HttpGet]
        public async Task<IActionResult> ResetUserPassword(long id)
        {
            using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    var user =await  _userManager.FindByIdAsync(id.ToString());
                    user.IsPasssReset = false;
                    await _userService.UpdateAsync(user,Accessor,User.GetUserId());
                    await _userManager.RemovePasswordAsync(user);
                    var psupdated=  await _userManager.AddPasswordAsync(user,"password");
                    if (psupdated.Succeeded)
                    {
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "Password reset successfully");
                    }
                    else {
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "something went wrong");
                    }
                    
                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "ResetUserPassword");
                    return JsonResponse.GenerateJsonResult(0, "");
                }
            }
        }


        #endregion

        #region Common

        public bool CheckEmail(string Email, long Id)
        {
            bool isExist;
            var result = _userService.GetSingle(x => x.Email.ToLower().Equals(Email.ToLower()) && x.IsActive == true );
            if (result != null)
            {

                isExist = result.Email.ToLower().Trim().Equals(Email.ToLower().Trim()) ? true : false;
                if (isExist && Id != 0)
                {
                    var resultExist = _userService.GetSingle(x => x.Email.ToLower().Equals(Email.ToLower()) && x.Id == Id );
                    return resultExist == null ? false : true;
                }
                else
                {
                    return result == null ? true : false;
                }
            }
            else
            {
                return result == null ? true : false;
            }
        }


        public async void SendEmail(ApplicationUser user, string CompanyName, string Password)
        {
            string returnUrl = null;
            returnUrl = returnUrl ?? Url.Content("~/");
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);
            string physicalUrl = Config.GetValue<string>("CommonProperty:PhysicalUrl");
            string emailTemplate = CommonMethod.ReadEmailTemplate(ErrorLog, _webHostEnvironment.WebRootPath, EmailTemplateFileList.ConfirmEmailEmployee, physicalUrl);
            emailTemplate = emailTemplate.Replace("{UserName}", user.FullName);
            emailTemplate = emailTemplate.Replace("{Password}", Password);
            emailTemplate = emailTemplate.Replace("{action_url}", callbackUrl);

            await _emailService.SendEmailAsyncByGmail(new SendEmailModel()
            {
                ToAddress = user.Email,
                Subject = "Welcome To " + CompanyName,
                BodyText = emailTemplate

            });
        }
        #endregion



    }
}
