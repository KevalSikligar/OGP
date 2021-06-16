using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OGP_Portal.Data.DbContext;
using OGP_Portal.Data.DbModel;
using OGP_Portal.Data.Extensions;
using OGP_Portal.Data.Utility;
using OGP_Portal.Service.Dtos;
using OGP_Portal.Service.Enums;
using OGP_Portal.Service.Implementation.BaseService;
using OGP_Portal.Service.Interface;

namespace OGP_Portal.Service.Implementation
{
	public class ForcastRepository : GenericRepository<Forcast>, IForcastService
    {
        private readonly OGP_PortalContext _context;
        public ForcastRepository(OGP_PortalContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ForcastDto>> GetForcastList(SqlParameter[] paraObjects)
        {
            var dataSet = await _context.GetQueryDatatableAsync(StoredProcedureList.GetForcastList, paraObjects);
            return Common.ConvertDataTable<ForcastDto>(dataSet.Tables[0]);
        }

    }
}