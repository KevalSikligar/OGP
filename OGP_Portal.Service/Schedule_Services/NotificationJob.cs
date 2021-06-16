using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using OGP_Portal.Data.DbContext;
using OGP_Portal.Service.Enums;
using OGP_Portal.Service.Interface;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGP_Portal.Service.Schedule_Services
{
    [DisallowConcurrentExecution]
    public class NotificationJob : IJob
    {
        private readonly ILogger<NotificationJob> _logger;
      
       


        public NotificationJob(ILogger<NotificationJob> logger)
        {
            _logger = logger;
           

        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Hello world!");


            var test = new LoadReport();
            var isDownload = test.DownloadReport();

            return  Task.CompletedTask;
        }
    }
}
