using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using OGP_Portal.Data.DbContext;
using OGP_Portal.Service.BackGroundService;
using OGP_Portal.Service.Dtos;
using OGP_Portal.Service.Enums;
using OGP_Portal.Service.Implementation;
using OGP_Portal.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGP_Portal.Service.Schedule_Services
{
    public class LoadReport
    {
        // private readonly UserManager<ApplicationUser> _userManager;




        private static StoredProcedureRepositoryBase _repositoryBase;
        private static SchedulerEmailService _schedulerEmail;

        public LoadReport()
        {
            _repositoryBase = new StoredProcedureRepositoryBase();
            _schedulerEmail = new SchedulerEmailService(new EmailSettingsGmail
            {
                SmtpUserName = "narolamma@gmail.com",
                SmtpHost = "smtp.gmail.com",
                SmtpSenderEmail = "narolamma@gmail.com",
                SmtpPassword = "Password123#",
                SmtpPort = 587,
                SenderName = "OGP Portal",
                SmtpEnableSsl = true
            });
        }


        public async Task<Task> DownloadReport()
        {

            try
            {
                var searchRecords = new SqlParameter { ParameterName = "@SearchRecords", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
                var Parameters = new List<SqlParameter>
                {

                    new SqlParameter("@IsDownload",SqlDbType.Bit){Value = true},
                    new SqlParameter("@iDisplayStart",SqlDbType.Int){Value = 0},
                    new SqlParameter("@iDisplayLength",SqlDbType.Int){Value = 100},
                    new SqlParameter("@SortColumn",SqlDbType.VarChar){Value = "Id"},
                    new SqlParameter("@SortDir",SqlDbType.VarChar){Value = "DESC"},
                    new SqlParameter("@Search",SqlDbType.NVarChar){Value = ""},
                    searchRecords
                }.ToArray();


                string qry = ProceduresList.GetExcelUserList;
                DataSet result = _repositoryBase.GetQueryDatatableAsync(qry, null, CommandType.StoredProcedure);
                var userList = _repositoryBase.CreateListFromTable<UserNameList>(result.Tables[0]).ToList();

                qry = StoredProcedureList.GetNewEntryList;

                DataSet Dataresult = _repositoryBase.GetQueryDatatableAsync(qry, Parameters, CommandType.StoredProcedure);
                var ExelResult = _repositoryBase.CreateListFromTable<BUserEntryListDto>(Dataresult.Tables[0]).ToList();


                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "BusinessPartnerReport.xlsx";

                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Report");

                    worksheet.Cell(1, 1).Value = "Date";
                    worksheet.Cell(1, 2).Value = "User Email";
                    worksheet.Cell(1, 3).Value = "Assembly Type";
                    worksheet.Cell(1, 4).Value = "ShellReceipt";
                    worksheet.Cell(1, 5).Value = "ShellWelding";
                    worksheet.Cell(1, 6).Value = "TopD_end_Receipt";
                    worksheet.Cell(1, 7).Value = "Fabrication";
                    worksheet.Cell(1, 8).Value = "Sent_to_paint";
                    worksheet.Cell(1, 9).Value = "Received_after_Paint";
                    worksheet.Cell(1, 10).Value = "Readiness";
                    worksheet.Cell(1, 11).Value = "BDE_Receipt";
                    worksheet.Cell(1, 12).Value = "BDE_readiness";
                    worksheet.Cell(1, 13).Value = "Shell_BDE";
                    worksheet.Cell(1, 14).Value = "Shell_BDE_Sent_to_Paint";
                    worksheet.Cell(1, 15).Value = "Shell_BDE_Received_after_paint";
                    worksheet.Cell(1, 16).Value = "SH_BDE_TDE";
                    worksheet.Cell(1, 17).Value = "Skid_Receipt";
                    worksheet.Cell(1, 18).Value = "Installation_Skid";
                    worksheet.Cell(1, 19).Value = "APT";
                    worksheet.Cell(1, 20).Value = "HPT";
                    worksheet.Cell(1, 21).Value = "Final_Paint";
                    worksheet.Cell(1, 22).Value = "Dispatch";
                    int ExcelRowID = 2;



                    long previousRowId = 0;
                    var currentColor = GetRandomColor();
                    var resultArray = ExelResult.ToArray();
                    for (int index = 0; index < ExelResult.Count; index++)
                    {

                        if (previousRowId == 0)
                        {
                            previousRowId = ExelResult[index].Id;
                            worksheet.Range(worksheet.Cell(ExcelRowID, 1), worksheet.Cell(ExcelRowID, 22)).Style.Fill.BackgroundColor = currentColor;
                        }

                        if (previousRowId != 0 && ExelResult[index].Id == previousRowId)
                        {
                            //worksheet.Row(ExcelRowID).Style.Fill.BackgroundColor = currentColor;
                            worksheet.Range(worksheet.Cell(ExcelRowID, 1), worksheet.Cell(ExcelRowID, 22)).Style.Fill.BackgroundColor = currentColor;
                            previousRowId = ExelResult[index].Id;
                        }
                        else
                        {
                            currentColor = GetRandomColor();
                            //worksheet.Row(ExcelRowID).Style.Fill.BackgroundColor = currentColor;
                            worksheet.Range(worksheet.Cell(ExcelRowID, 1), worksheet.Cell(ExcelRowID, 22)).Style.Fill.BackgroundColor = currentColor;
                            previousRowId = ExelResult[index].Id;
                        }



                        worksheet.Cell(ExcelRowID, 1).Value = ExelResult[index].CreatedDateSTR;
                        worksheet.Cell(ExcelRowID, 2).Value = ExelResult[index].username;
                        worksheet.Cell(ExcelRowID, 3).Value = ExelResult[index].asslytype;
                        worksheet.Cell(ExcelRowID, 4).Value = ExelResult[index].ShellReceipt;
                        worksheet.Cell(ExcelRowID, 5).Value = ExelResult[index].ShellWelding;
                        worksheet.Cell(ExcelRowID, 6).Value = ExelResult[index].TopD_end_Receipt;
                        worksheet.Cell(ExcelRowID, 7).Value = ExelResult[index].Fabrication;
                        worksheet.Cell(ExcelRowID, 8).Value = ExelResult[index].Sent_to_paint;
                        worksheet.Cell(ExcelRowID, 9).Value = ExelResult[index].Received_after_Paint;
                        worksheet.Cell(ExcelRowID, 10).Value = ExelResult[index].Readiness;
                        worksheet.Cell(ExcelRowID, 11).Value = ExelResult[index].BDE_Receipt;
                        worksheet.Cell(ExcelRowID, 12).Value = ExelResult[index].BDE_readiness;
                        worksheet.Cell(ExcelRowID, 13).Value = ExelResult[index].Shell_BDE;
                        worksheet.Cell(ExcelRowID, 14).Value = ExelResult[index].Shell_BDE_Sent_to_Paint;
                        worksheet.Cell(ExcelRowID, 15).Value = ExelResult[index].Shell_BDE_Received_after_paint;
                        worksheet.Cell(ExcelRowID, 16).Value = ExelResult[index].SH_BDE_TDE;
                        worksheet.Cell(ExcelRowID, 17).Value = ExelResult[index].Skid_Receipt;
                        worksheet.Cell(ExcelRowID, 18).Value = ExelResult[index].Installation_Skid;
                        worksheet.Cell(ExcelRowID, 19).Value = ExelResult[index].APT;
                        worksheet.Cell(ExcelRowID, 20).Value = ExelResult[index].HPT;
                        worksheet.Cell(ExcelRowID, 21).Value = ExelResult[index].Final_Paint;
                        worksheet.Cell(ExcelRowID, 22).Value = ExelResult[index].Dispatch;



                        ExcelRowID = ExcelRowID + 1;

                    }


                    var isRowCompleted = false;
                    foreach (var row in workbook.Worksheets.FirstOrDefault().Rows())
                    {
                        isRowCompleted = false;
                        foreach (var cell in row.Cells())
                        {
                            int current = -1;
                            try
                            {
                                current = Convert.ToInt32(cell.Value);
                            }
                            catch (Exception e) { }
                            finally
                            {

                                if (current == 0 && !isRowCompleted)
                                {
                                    cell.CellLeft().Style.Fill.BackgroundColor = XLColor.Yellow;
                                    isRowCompleted = true;
                                }
                                else
                                {

                                }
                            }

                        }
                    }




                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(fileName);
                        //var content = stream.ToArray();
                        //File (content, contentType, fileName);
                        // return Content("TEst");
                    }

                    string EmailTemplate = _schedulerEmail.ReadEmailTemplate(@"wwwroot\EmailTemplate", "UserReport.html", "");
                    EmailTemplate = EmailTemplate.Replace("{Date}", DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy"));
                    var Subject = "OGP report as on " + DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy");

                    foreach (var user in userList)
                    {
                        await Task.Run(() => _schedulerEmail.SendEmailAsyncByGmail(new OGP_Portal.Service.BackGroundService.SendEmailModel()
                        {
                            BodyText = EmailTemplate,
                            Subject = Subject,
                            ToAddress = user.UserName, // samplemails - Pharmacy List 
                            ToDisplayName = "Team OGP", // OGP Name,
                            Attachment = File.ReadAllBytes(fileName),
                            AttachmentName = "BusinessPartnerReport.xlsx",

                        }));

                    }

                    if (File.Exists(fileName))
                        File.Delete(fileName);

                }

                return Task.CompletedTask;

            }
            catch (Exception e)
            {
                return Task.CompletedTask;
            }

        }


        public XLColor GetRandomColor()
        {
            var colorlist = new List<XLColor>();
            colorlist.Add(XLColor.Lime);
            colorlist.Add(XLColor.Aqua);
            colorlist.Add(XLColor.Lavender);
            colorlist.Add(XLColor.LightGreen);
            colorlist.Add(XLColor.Iceberg);
            colorlist.Add(XLColor.LightBrown);
            colorlist.Add(XLColor.LightPink);
            colorlist.Add(XLColor.LightSeaGreen);

            var _ram = new Random();
            var resultIndex = _ram.Next(0, colorlist.Count());
            return colorlist[resultIndex];

        }
    }
}
