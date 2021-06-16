using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OGP_Portal.Common;
using OGP_Portal.Data.DbContext;
using OGP_Portal.Data.DbModel;
using OGP_Portal.Models;
using OGP_Portal.Service.Dtos;
using OGP_Portal.Service.Enums;
using OGP_Portal.Service.Exceptions;
using OGP_Portal.Service.Interface;
using OGP_Portal.Utility;

namespace OGP_Portal.Areas.BusinessUser.Controllers
{
  

    [Authorize(Roles = UserRoles.BusinessUser), Area("BusinessUser")]
    public class BusinessUserController : BaseController<BusinessUserController>
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEntryService _entryService;
        private readonly IB_PartnerService _partnerService;
        #endregion

        #region Ctor
        public BusinessUserController(UserManager<ApplicationUser> userManager, IEntryService entryService,
            IB_PartnerService partnerService
            )
        {
            _userManager = userManager;
            _entryService = entryService;
            _partnerService = partnerService;
        }
        #endregion

        #region Methods

        public async Task<IActionResult> Index()
        {
            var userResult = await _userManager.FindByIdAsync(User.GetUserId().ToString());
            if (userResult.IsPasssReset)
            {
                return View();
            }
            else
            {

                return RedirectToAction("GetReset", "BusinessUser", new ResetPasswordDto { Id = userResult.Id });

            }

        }

        [HttpGet]
        public IActionResult _BHistory(long partnerId)
        {

            return View(@"Components/_BHistory", partnerId);
        }

        [HttpGet]
        public async Task<IActionResult> GetEntryList(JQueryDataTableParamModel param)
        {
            using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;
                    var allList = await _entryService.GetNewEntryList(parameters.ToArray());

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
        public async Task<IActionResult> GetUserSide_B_HistoryList(JQueryDataTableParamModel param,long partnerId)
        {
            using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;
                    parameters.Insert(0, new SqlParameter("@UserId", SqlDbType.BigInt) { Value = partnerId });
                    var allList = await _entryService.GetUserSide_B_HistoryList(parameters.ToArray());

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
                    ErrorLog.AddErrorLog(ex, "GetUserSide_B_HistoryList");
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
        #endregion

        #region Common

        public IActionResult GetReset(ResetPasswordDto model)
        {
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Resetpassword(ResetPasswordDto model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(User.GetUserId().ToString());

                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.IsPasssReset = true;
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("GetReset");
                    }
                }
                else
                {
                    return RedirectToAction("GetReset");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("GetReset");

            }


        }
        #endregion


    }
}
