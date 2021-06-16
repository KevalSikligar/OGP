using OGP_Portal.Data.DbContext;
using OGP_Portal.Data.DbModel;
using OGP_Portal.Service.Implementation.BaseService;
using OGP_Portal.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace OGP_Portal.Service.Implementation
{
   public  class Entry_LogRepository : GenericRepository<Entry_Log>, IEntryLogService
    {
        private readonly OGP_PortalContext _context;
        public Entry_LogRepository(OGP_PortalContext context) : base(context)
        {
            _context = context;
        }


    }
}