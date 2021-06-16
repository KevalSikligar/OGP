using OGP_Portal.Data;
using OGP_Portal.Data.DbContext;
using OGP_Portal.Data.DbModel;
using OGP_Portal.Service.Implementation.BaseService;
using OGP_Portal.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JunTechnology.Service.Implementation
{
    public class ErrorLogRepository : GenericRepository<ErrorLog>, IErrorLogService
    {
        private readonly OGP_PortalContext _db;
        public ErrorLogRepository(OGP_PortalContext db) : base(db)
        {
            _db = db;
        }

        public void AddErrorLog(Exception ex, string appType)
        {
            try
            {
                var errorlog = new ErrorLog
                {
                    Source = ex.Source,
                    Path = appType,
                    TargetSite = ex.TargetSite.Name,
                    Type = ex.GetType().Name,
                    Message = ex.Message,// + entityValidationError,
                    Stack = ex.StackTrace,
                    LogDate = DateTime.UtcNow
                };
                Add(errorlog);
                Save();
            }
            catch (Exception e)
            {
                WriteToLogFile(e.Message);
                throw;
            }

        }
        private static void WriteToLogFile(string message)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine("------------------------------------------------------------------");
                    sw.WriteLine(DateTime.UtcNow.ToShortDateString());
                    sw.WriteLine("Error in Error Log");
                    sw.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine("------------------------------------------------------------------");
                    sw.WriteLine(DateTime.UtcNow.ToShortDateString());
                    sw.WriteLine("Error in Error Log");
                    sw.WriteLine(message);
                }
            }
        }


    }
}
