using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<BalanceDto> GetBalance(SqlParameter[] paraObjects)
        {
            var dataSet = await _context.GetQueryDatatableAsync(StoredProcedureList.GetBalance, paraObjects);

            return Common.ConvertDataTable<BalanceDto>(dataSet.Tables[0]).FirstOrDefault();
        }

        public async Task<FDD_DTO> GetFDDList(SqlParameter[] paraObjects)
        {
            var model = new FDD_DTO();
            model.partnerFDDs = new List<PartnerFDD>();
            model.fddDetails = new List<FddDetail>();
            var dataSet = await _context.GetQueryDatatableAsync(StoredProcedureList.GetFDDList, paraObjects);
            var first  = Common.ConvertDataTable<PartnerFDD>(dataSet.Tables[0]);
            var second = Common.ConvertDataTable<FddDetail>(dataSet.Tables[1]);

            model.partnerFDDs.AddRange(first);
            model.fddDetails.AddRange(second);

            return model;
        }

    }
}