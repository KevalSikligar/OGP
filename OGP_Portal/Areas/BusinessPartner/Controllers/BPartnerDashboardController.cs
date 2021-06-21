using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace OGP_Portal.Areas.BusinessPartner.Controllers
{

	[Authorize(Roles = UserRoles.BusinessPartner), Area("BusinessPartner")]
	public class BPartnerDashboardController : BaseController<BPartnerDashboardController>
	{
		#region Fields
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IEntryService _entryService;
		private readonly IB_PartnerService _partnerService;
		private readonly IEntryLogService _entry_LogService;
		private readonly IForcastService _forcastService;
		#endregion

		#region Ctor
		public BPartnerDashboardController(IForcastService forcastService, IEntryLogService entryLogService, UserManager<ApplicationUser> userManager, IEntryService entryService,
			IB_PartnerService partnerService
			)
		{
			_userManager = userManager;
			_entryService = entryService;
			_partnerService = partnerService;
			_entry_LogService = entryLogService;
			_forcastService = forcastService;
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

				return RedirectToAction("GetReset", "BPartnerDashboard", new ResetPasswordDto { Id = userResult.Id });

			}

		}



		[HttpGet]
		public async Task<IActionResult> _AddEditEntry(long id)
		{
			var assylist = new List<AssyTypeDto>();

			assylist.Add(new AssyTypeDto { Id = 1, Name = "Ganerator" });
			assylist.Add(new AssyTypeDto { Id = 2, Name = "Receiver" });

			ViewBag.assylist = assylist.Select(x => new SelectListItem()
			{
				Text = x.Name,
				Value = x.Id.ToString()
			}).OrderBy(x => x.Text);

			var partnerUSer = await _userManager.FindByIdAsync(User.GetUserId().ToString());
			var OGP_LIMIT = _partnerService.GetSingle(x => x.UserId == partnerUSer.Id);
			if (id == 0) return base.View(@"Components/_AddEditEntry", new EntryDto { Id = id, OGP_Limit = OGP_LIMIT.Quantity_of_OGP });
			var result = _entryService.GetSingle(x => x.Id == id);
			var tempView = Mapper.Map<EntryDto>(result);

			return View(@"Components/_AddEditEntry", tempView);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditEntry(EntryDto model)
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
					if (model.Id == 0)
					{

						var uResult = await _userManager.FindByIdAsync(User.GetUserId().ToString());

						Entry entry = new Entry();
						entry.VendorName = uResult.Email;
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
						entry.PartnerId = User.GetUserId();
						entry.IsActive = true;
						var userResult = await _entryService.InsertAsync(entry, Accessor, User.GetUserId());

						if (userResult != null)
						{
							txscope.Complete();
							return JsonResponse.GenerateJsonResult(1, "Entries Inserted.");
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
					parameters.Insert(0, new SqlParameter("@UserId", SqlDbType.BigInt) { Value = User.GetUserId() });
					var allList = await _entryService.GetBusinessPartnerEntryList(parameters.ToArray());
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
						return RedirectToAction("GetOGPEntry");
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


		[HttpGet]
		public async Task<IActionResult> GetOGPEntry()
		{
			var userResult = await _userManager.FindByIdAsync(User.GetUserId().ToString());
			if (userResult.IsPasssReset)
			{
				return View();
			}
			else
			{

				return RedirectToAction("GetReset", "BPartnerDashboard", new ResetPasswordDto { Id = userResult.Id });

			}
		}

		[HttpPost]
		public async Task<IActionResult> PostOGPEntry(OGPEntryDTO entry)
		{
			using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				try
				{

					var isExist = _entryService.GetAll(x => x.PartnerId == User.GetUserId());
					var g_log = new Entry_Log();

					g_log.ShellReceipt = entry.G_ShellReceipt;
					g_log.ShellWelding = entry.G_ShellWelding;
					g_log.TopD_end_Receipt = entry.G_TopD_end_Receipt;
					g_log.Fabrication = entry.G_Fabrication;
					g_log.Sent_to_paint = entry.G_Sent_to_paint;
					g_log.Received_after_Paint = entry.G_Received_after_Paint;
					g_log.Readiness = entry.G_Readiness;
					g_log.BDE_Receipt = entry.G_BDE_Receipt;
					g_log.BDE_readiness = entry.G_BDE_readiness;
					g_log.Shell_BDE = entry.G_Shell_BDE;
					g_log.Shell_BDE_Sent_to_Paint = entry.G_Shell_BDE_Sent_to_Paint;
					g_log.Shell_BDE_Received_after_paint = entry.G_Shell_BDE_Received_after_paint;
					g_log.SH_BDE_TDE = entry.G_SH_BDE_TDE;
					g_log.Skid_Receipt = entry.G_Skid_Receipt;
					g_log.Installation_Skid = entry.G_Installation_Skid;
					g_log.APT = entry.G_APT;
					g_log.HPT = entry.G_HPT;
					g_log.Final_Paint = entry.G_Final_Paint;
					g_log.Dispatch = entry.G_Dispatch;
					g_log.PartnerId = User.GetUserId();
					g_log.AsslyType = 1;

					var g_logResult = await _entry_LogService.InsertAsync(g_log, Accessor, User.GetUserId());

					var r_log = new Entry_Log();

					r_log.ShellReceipt = entry.R_ShellReceipt;
					r_log.ShellWelding = entry.R_ShellWelding;
					r_log.TopD_end_Receipt = entry.R_TopD_end_Receipt;
					r_log.Fabrication = entry.R_Fabrication;
					r_log.Sent_to_paint = entry.R_Sent_to_paint;
					r_log.Received_after_Paint = entry.R_Received_after_Paint;
					r_log.Readiness = entry.R_Readiness;
					r_log.BDE_Receipt = entry.R_BDE_Receipt;
					r_log.BDE_readiness = entry.R_BDE_readiness;
					r_log.Shell_BDE = entry.R_Shell_BDE;
					r_log.Shell_BDE_Sent_to_Paint = entry.R_Shell_BDE_Sent_to_Paint;
					r_log.Shell_BDE_Received_after_paint = entry.R_Shell_BDE_Received_after_paint;
					r_log.SH_BDE_TDE = entry.R_SH_BDE_TDE;
					r_log.Skid_Receipt = entry.R_Skid_Receipt;
					r_log.Installation_Skid = entry.R_Installation_Skid;
					r_log.APT = entry.R_APT;
					r_log.HPT = entry.R_HPT;
					r_log.Final_Paint = entry.R_Final_Paint;
					r_log.Dispatch = entry.R_Dispatch;
					r_log.PartnerId = User.GetUserId();
					r_log.AsslyType = 2;

					var r_logResult = await _entry_LogService.InsertAsync(r_log, Accessor, User.GetUserId());
					if (isExist.Count() != 0)
					{


						// Log History




						var g_model = _entryService.GetSingle(x => x.PartnerId == User.GetUserId() && x.AsslyType == 1);
						g_model.ShellReceipt = entry.G_ShellReceipt;
						g_model.ShellWelding = entry.G_ShellWelding;
						g_model.TopD_end_Receipt = entry.G_TopD_end_Receipt;
						g_model.Fabrication = entry.G_Fabrication;
						g_model.Sent_to_paint = entry.G_Sent_to_paint;
						g_model.Received_after_Paint = entry.G_Received_after_Paint;
						g_model.Readiness = entry.G_Readiness;
						g_model.BDE_Receipt = entry.G_BDE_Receipt;
						g_model.BDE_readiness = entry.G_BDE_readiness;
						g_model.Shell_BDE = entry.G_Shell_BDE;
						g_model.Shell_BDE_Sent_to_Paint = entry.G_Shell_BDE_Sent_to_Paint;
						g_model.Shell_BDE_Received_after_paint = entry.G_Shell_BDE_Received_after_paint;
						g_model.SH_BDE_TDE = entry.G_SH_BDE_TDE;
						g_model.Skid_Receipt = entry.G_Skid_Receipt;
						g_model.Installation_Skid = entry.G_Installation_Skid;
						g_model.APT = entry.G_APT;
						g_model.HPT = entry.G_HPT;
						g_model.Final_Paint = entry.G_Final_Paint;
						g_model.Dispatch = entry.G_Dispatch;
						g_model.PartnerId = User.GetUserId();
						g_model.AsslyType = 1;

						var g_insertResult = await _entryService.UpdateAsync(g_model, Accessor, User.GetUserId());


						var r_model = _entryService.GetSingle(x => x.PartnerId == User.GetUserId() && x.AsslyType == 2);
						r_model.ShellReceipt = entry.R_ShellReceipt;
						r_model.ShellWelding = entry.R_ShellWelding;
						r_model.TopD_end_Receipt = entry.R_TopD_end_Receipt;
						r_model.Fabrication = entry.R_Fabrication;
						r_model.Sent_to_paint = entry.R_Sent_to_paint;
						r_model.Received_after_Paint = entry.R_Received_after_Paint;
						r_model.Readiness = entry.R_Readiness;
						r_model.BDE_Receipt = entry.R_BDE_Receipt;
						r_model.BDE_readiness = entry.R_BDE_readiness;
						r_model.Shell_BDE = entry.R_Shell_BDE;
						r_model.Shell_BDE_Sent_to_Paint = entry.R_Shell_BDE_Sent_to_Paint;
						r_model.Shell_BDE_Received_after_paint = entry.R_Shell_BDE_Received_after_paint;
						r_model.SH_BDE_TDE = entry.R_SH_BDE_TDE;
						r_model.Skid_Receipt = entry.R_Skid_Receipt;
						r_model.Installation_Skid = entry.R_Installation_Skid;
						r_model.APT = entry.R_APT;
						r_model.HPT = entry.R_HPT;
						r_model.Final_Paint = entry.R_Final_Paint;
						r_model.Dispatch = entry.R_Dispatch;
						r_model.PartnerId = User.GetUserId();
						r_model.AsslyType = 2;

						var r_insertResult = await _entryService.UpdateAsync(r_model, Accessor, User.GetUserId());

						if (g_insertResult != null && r_insertResult != null)
						{
							txscope.Complete();
							return JsonResponse.GenerateJsonResult(1, "Entry Updated");
						}
						else
						{
							txscope.Dispose();
							ErrorLog.AddErrorLog(null, "GetOGPEntry");
							return JsonResponse.GenerateJsonResult(0, "");
						}

					}
					else
					{





						var g_model = new Entry();

						g_model.ShellReceipt = entry.G_ShellReceipt;
						g_model.ShellWelding = entry.G_ShellWelding;
						g_model.TopD_end_Receipt = entry.G_TopD_end_Receipt;
						g_model.Fabrication = entry.G_Fabrication;
						g_model.Sent_to_paint = entry.G_Sent_to_paint;
						g_model.Received_after_Paint = entry.G_Received_after_Paint;
						g_model.Readiness = entry.G_Readiness;
						g_model.BDE_Receipt = entry.G_BDE_Receipt;
						g_model.BDE_readiness = entry.G_BDE_readiness;
						g_model.Shell_BDE = entry.G_Shell_BDE;
						g_model.Shell_BDE_Sent_to_Paint = entry.G_Shell_BDE_Sent_to_Paint;
						g_model.Shell_BDE_Received_after_paint = entry.G_Shell_BDE_Received_after_paint;
						g_model.SH_BDE_TDE = entry.G_SH_BDE_TDE;
						g_model.Skid_Receipt = entry.G_Skid_Receipt;
						g_model.Installation_Skid = entry.G_Installation_Skid;
						g_model.APT = entry.G_APT;
						g_model.HPT = entry.G_HPT;
						g_model.Final_Paint = entry.G_Final_Paint;
						g_model.Dispatch = entry.G_Dispatch;
						g_model.PartnerId = User.GetUserId();
						g_model.AsslyType = 1;

						var g_insertResult = await _entryService.InsertAsync(g_model, Accessor, User.GetUserId());


						var r_model = new Entry();

						r_model.ShellReceipt = entry.R_ShellReceipt;
						r_model.ShellWelding = entry.R_ShellWelding;
						r_model.TopD_end_Receipt = entry.R_TopD_end_Receipt;
						r_model.Fabrication = entry.R_Fabrication;
						r_model.Sent_to_paint = entry.R_Sent_to_paint;
						r_model.Received_after_Paint = entry.R_Received_after_Paint;
						r_model.Readiness = entry.R_Readiness;
						r_model.BDE_Receipt = entry.R_BDE_Receipt;
						r_model.BDE_readiness = entry.R_BDE_readiness;
						r_model.Shell_BDE = entry.R_Shell_BDE;
						r_model.Shell_BDE_Sent_to_Paint = entry.R_Shell_BDE_Sent_to_Paint;
						r_model.Shell_BDE_Received_after_paint = entry.R_Shell_BDE_Received_after_paint;
						r_model.SH_BDE_TDE = entry.R_SH_BDE_TDE;
						r_model.Skid_Receipt = entry.R_Skid_Receipt;
						r_model.Installation_Skid = entry.R_Installation_Skid;
						r_model.APT = entry.R_APT;
						r_model.HPT = entry.R_HPT;
						r_model.Final_Paint = entry.R_Final_Paint;
						r_model.Dispatch = entry.R_Dispatch;
						r_model.PartnerId = User.GetUserId();
						r_model.AsslyType = 2;

						var r_insertResult = await _entryService.InsertAsync(r_model, Accessor, User.GetUserId());
						if (g_insertResult != null && r_insertResult != null)
						{
							txscope.Complete();
							return JsonResponse.GenerateJsonResult(1, "Entry Inserted");
						}
						else
						{
							txscope.Dispose();
							ErrorLog.AddErrorLog(null, "GetOGPEntry");
							return JsonResponse.GenerateJsonResult(0, "");
						}

					}
				}
				catch (Exception ex)
				{
					txscope.Dispose();
					ErrorLog.AddErrorLog(ex, "GetOGPEntry");
					return JsonResponse.GenerateJsonResult(0, "");
				}
			}
		}


		[HttpGet]
		public async Task<IActionResult> FatchOGP()
		{
			var g_model = _entryService.GetAll(x => x.PartnerId == User.GetUserId() && x.AsslyType == 1).FirstOrDefault();
			var r_model = _entryService.GetAll(x => x.PartnerId == User.GetUserId() && x.AsslyType == 2).FirstOrDefault();

			var MaxOGP = _partnerService.GetSingle(x => x.UserId == User.GetUserId()).Quantity_of_OGP;
			
			var model = new OGPEntryDTO();
			model.MaxOGP = MaxOGP;
			var user = await _userManager.FindByIdAsync(User.GetUserId().ToString());

			var Parameters = new List<SqlParameter>
				{
					new SqlParameter("@userId",SqlDbType.BigInt){Value = Convert.ToInt64(User.GetUserId())}

				};
			var balance =await  _forcastService.GetBalance(Parameters.ToArray());



			model.PartnerName = user.FirstName;
			model.Balance = balance.Balance;
			if (g_model != null)
			{
				model.G_ShellReceipt = g_model.ShellReceipt;
				model.G_ShellWelding = g_model.ShellWelding;
				model.G_TopD_end_Receipt = g_model.TopD_end_Receipt;
				model.G_Fabrication = g_model.Fabrication;
				model.G_Sent_to_paint = g_model.Sent_to_paint;
				model.G_Received_after_Paint = g_model.Received_after_Paint;
				model.G_Readiness = g_model.Readiness;
				model.G_BDE_Receipt = g_model.BDE_Receipt;
				model.G_BDE_readiness = g_model.BDE_readiness;
				model.G_Shell_BDE = g_model.Shell_BDE;
				model.G_Shell_BDE_Sent_to_Paint = g_model.Shell_BDE_Sent_to_Paint;
				model.G_Shell_BDE_Received_after_paint = g_model.Shell_BDE_Received_after_paint;
				model.G_SH_BDE_TDE = g_model.SH_BDE_TDE;
				model.G_Skid_Receipt = g_model.Skid_Receipt;
				model.G_Installation_Skid = g_model.Installation_Skid;
				model.G_APT = g_model.APT;
				model.G_HPT = g_model.HPT;
				model.G_Final_Paint = g_model.Final_Paint;
				model.G_Dispatch = g_model.Dispatch;


			}

			if (r_model != null)
			{
				model.R_ShellReceipt = r_model.ShellReceipt;
				model.R_ShellWelding = r_model.ShellWelding;
				model.R_TopD_end_Receipt = r_model.TopD_end_Receipt;
				model.R_Fabrication = r_model.Fabrication;
				model.R_Sent_to_paint = r_model.Sent_to_paint;
				model.R_Received_after_Paint = r_model.Received_after_Paint;
				model.R_Readiness = r_model.Readiness;
				model.R_BDE_Receipt = r_model.BDE_Receipt;
				model.R_BDE_readiness = r_model.BDE_readiness;
				model.R_Shell_BDE = r_model.Shell_BDE;
				model.R_Shell_BDE_Sent_to_Paint = r_model.Shell_BDE_Sent_to_Paint;
				model.R_Shell_BDE_Received_after_paint = r_model.Shell_BDE_Received_after_paint;
				model.R_SH_BDE_TDE = r_model.SH_BDE_TDE;
				model.R_Skid_Receipt = r_model.Skid_Receipt;
				model.R_Installation_Skid = r_model.Installation_Skid;
				model.R_APT = r_model.APT;
				model.R_HPT = r_model.HPT;
				model.R_Final_Paint = r_model.Final_Paint;
				model.R_Dispatch = r_model.Dispatch;

			}


			return JsonResponse.GenerateJsonResult(0, "", model);
		}


		[HttpPost]
		public async Task<IActionResult> AddForcast(ForcastDto model)
		{
			using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				try
				{
					var obj = new Forcast();
					obj.Quantity = model.Quantity;
					obj.UserId = User.GetUserId();
					obj.ForcastDate = Convert.ToDateTime(model.ForcastDate);

					var insertResult = await _forcastService.InsertAsync(obj, Accessor, User.GetUserId());

					if (insertResult != null)
					{
						txscope.Complete();
						return JsonResponse.GenerateJsonResult(1, "Record added");
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
					ErrorLog.AddErrorLog(ex, "AddPlan");
					return JsonResponse.GenerateJsonResult(0, "System error");

				}
			}
		}

		[HttpPost]
		public async Task<IActionResult> UpdateForcast(long forcastId, int quantity)
		{
			var obj = _forcastService.GetSingle(x => x.Id == forcastId);
			obj.Quantity = quantity;
			var updateResult = await _forcastService.UpdateAsync(obj, Accessor, User.GetUserId());
			if(updateResult!=null)
				return JsonResponse.GenerateJsonResult(1, "Record Updated");
			else
				return JsonResponse.GenerateJsonResult(0, "Something went wrong !");
		}


		[HttpGet]
		public IActionResult CheckForcast(string forcastDate)
		{
			var isExist = _forcastService.GetAll(x => x.UserId == User.GetUserId() && x.ForcastDate == Convert.ToDateTime(forcastDate));
			if(isExist.Count()>0)
				return JsonResponse.GenerateJsonResult(0, "", isExist.FirstOrDefault());
			else
				return JsonResponse.GenerateJsonResult(1, "");


		}

		[HttpGet]
		public async Task<IActionResult> GetForcastList(JQueryDataTableParamModel param)
		{
			using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				try
				{
					var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;
					parameters.Insert(0, new SqlParameter("@UserId", SqlDbType.BigInt) { Value = User.GetUserId() });
					var allList = await _forcastService.GetForcastList(parameters.ToArray());
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
					ErrorLog.AddErrorLog(ex, "GetForcastList");
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
		public async Task<IActionResult> GetLogEntryList(JQueryDataTableParamModel param)
		{
			using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				try
				{
					var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;
					parameters.Insert(0, new SqlParameter("@UserId", SqlDbType.BigInt) { Value = User.GetUserId() });
					var allList = await _entryService.GetLogEntryList(parameters.ToArray());
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
					ErrorLog.AddErrorLog(ex, "GetLogEntryList");
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


	}
}
