using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using OGP_Portal.Common;
using OGP_Portal.Models;
using OGP_Portal.Service.Dtos;
using OGP_Portal.Service.Enums;
using OGP_Portal.Service.Exceptions;
using OGP_Portal.Service.Interface;
using OGP_Portal.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace OGP_Portal.Areas.Admin.Controllers
{
    [Authorize(Roles = UserRoles.Administrator), Area("Admin")]
    public class AdminDashboardController : BaseController<AdminDashboardController>
    {
        private readonly IEntryService _entryService;
        private readonly IEntryLogService _entry_logService;
        private readonly IB_PartnerService  _partnerService;
        private readonly IUserService   _userService;
        #region Fields

        #endregion

        #region Ctor
        public AdminDashboardController(IEntryLogService entry_logService,IEntryService entryService, IB_PartnerService partnerService,IUserService userService)
        {
            _entryService = entryService;
            _entry_logService = entry_logService;
            _partnerService = partnerService;
            _userService = userService;
        }
        #endregion

        #region Methods

        public IActionResult Index()
        {


            return View();
        }

        [HttpGet]
        public IActionResult _AddEditEntry(long Id,long EntryId)
        {

            var assylist = new List<AssyTypeDto>();

            assylist.Add(new AssyTypeDto { Id = 1, Name = "Generator" });
            assylist.Add(new AssyTypeDto { Id = 2, Name = "Receiver" });

            ViewBag.assylist = assylist.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).OrderBy(x => x.Text);
            var OGP_LIMIT = _partnerService.GetSingle(x => x.UserId == Id);
           // var entryObj = _entryService.GetSingle(x => x.Id == EntryId);
            var entryObj = _entry_logService.GetSingle(x => x.Id == EntryId);
           
           

            var tempView = new EntryDto();
            tempView.VendorName = _userService.GetSingle(x=>x.Id==OGP_LIMIT.UserId).FirstName;
            tempView.AsslyType = entryObj.AsslyType;
            tempView.ShellReceipt = entryObj.ShellReceipt;
            tempView.TopD_end_Receipt = entryObj.TopD_end_Receipt;
            tempView.Fabrication = entryObj.Fabrication;
            tempView.Sent_to_paint = entryObj.Sent_to_paint;
            tempView.Received_after_Paint = entryObj.Received_after_Paint;
            tempView.Readiness = entryObj.Readiness;
            tempView.BDE_Receipt = entryObj.BDE_Receipt;
            tempView.BDE_readiness = entryObj.BDE_readiness;
            tempView.Shell_BDE = entryObj.Shell_BDE;
            tempView.Shell_BDE_Sent_to_Paint = entryObj.Shell_BDE_Sent_to_Paint;
            tempView.Shell_BDE_Received_after_paint = entryObj.Shell_BDE_Received_after_paint;
            tempView.SH_BDE_TDE = entryObj.SH_BDE_TDE;
            tempView.Skid_Receipt = entryObj.Skid_Receipt;
            tempView.Installation_Skid = entryObj.Installation_Skid;
            tempView.APT = entryObj.APT;
            tempView.HPT = entryObj.HPT;
            tempView.Installation_Skid = entryObj.Installation_Skid;
            tempView.Final_Paint = entryObj.Final_Paint;
            tempView.Dispatch = entryObj.Dispatch;
            tempView.Final_Paint = entryObj.Final_Paint;
            tempView.OGP_Limit = OGP_LIMIT.Quantity_of_OGP;
            tempView.EntryId = entryObj.Id;
            
            return View(@"Components/_AddEditEntry", tempView);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditEntry(EntryDto model)
        {
            using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        txscope.Dispose();
                        return RedirectToAction("_AddEditBPartner", model.EntryId);
                    }
                  
                        var entry = _entry_logService.GetSingle(x => x.Id == model.EntryId);
                       
                        entry.VendorName = model.VendorName;
                        entry.AsslyType = model.AsslyType;
                        entry.ShellReceipt = model.ShellReceipt;
                        entry.ShellWelding = model.ShellWelding;
                        entry.TopD_end_Receipt = model.TopD_end_Receipt;
                        entry.Fabrication = model.Fabrication;
                        entry.Sent_to_paint = model.Sent_to_paint;
                        entry.Received_after_Paint = model.Received_after_Paint;
                        entry.Readiness = model.Readiness;
                        entry.BDE_Receipt = model.BDE_Receipt;
                        entry.BDE_readiness = model.BDE_readiness;
                        entry.Shell_BDE = model.Shell_BDE;
                        entry.Shell_BDE_Sent_to_Paint = model.Shell_BDE_Sent_to_Paint;
                        entry.Shell_BDE_Received_after_paint = model.Shell_BDE_Received_after_paint;
                        entry.SH_BDE_TDE = model.SH_BDE_TDE;
                        entry.Skid_Receipt = model.Skid_Receipt;
                        entry.Installation_Skid = model.Installation_Skid;
                        entry.APT = model.APT;
                        entry.HPT = model.HPT;
                        entry.Final_Paint = model.Final_Paint;
                        entry.Dispatch = model.Dispatch;
                        
                        var userResult = await _entry_logService.UpdateAsync(entry, Accessor, User.GetUserId());

                        if (userResult != null)
                        {
                            txscope.Complete();
                            return JsonResponse.GenerateJsonResult(1, "Entries updated.");
                        }
                        else
                        {
                            txscope.Dispose();
                            return JsonResponse.GenerateJsonResult(0, "Something went wrong");
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
        public async Task<IActionResult> GetEntryList(JQueryDataTableParamModel param)
        {
            using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;
                   
                    var allList = await _entryService.GetAdminDasboardEntryList(parameters.ToArray());


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
        #endregion

        #region Common
        #endregion


    }
}
