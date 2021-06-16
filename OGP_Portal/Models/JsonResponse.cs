using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OGP_Portal.Models
{
   
        public class JsonResponse
        {
            public IActionResult GenerateJsonResult(int status, string message = null, object data = null)
            {
                return new JsonResult(new JsonResponseHelper
                {
                    Status = status,
                    Data = data,
                    Message = message
                });
            }
        }
        public class DatatableResponseHelper
        {
            public int TotalElements { get; set; }
            public object Data { get; set; }
        }

        public class JsonResponseHelper
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }
    }

