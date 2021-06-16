using Microsoft.Data.SqlClient;
using OGP_Portal.Data.DbContext;
using OGP_Portal.Data.DbModel;
using OGP_Portal.Data.Extensions;
using OGP_Portal.Data.Utility;
using OGP_Portal.Service.Dtos;
using OGP_Portal.Service.Enums;
using OGP_Portal.Service.Implementation.BaseService;
using OGP_Portal.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OGP_Portal.Service.Implementation
{
  public   class EntryRepository : GenericRepository<Entry>, IEntryService
    {
        private readonly OGP_PortalContext _context;
        public EntryRepository(OGP_PortalContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<EntryDto>> GetEntryList(SqlParameter[] paraObjects)
        {
            var dataSet = await _context.GetQueryDatatableAsync(StoredProcedureList.GetEntryList, paraObjects);
            return Common.ConvertDataTable<EntryDto>(dataSet.Tables[0]);
        }
        public async Task<List<OGPEntryDTO>> GetBalance(SqlParameter[] paraObjects)
        {
            var dataSet = await _context.GetQueryDatatableAsync(StoredProcedureList.GetBalance, paraObjects);
            return Common.ConvertDataTable<OGPEntryDTO>(dataSet.Tables[0]);
        }

        public async Task<List<BUserEntryListDto>> GetNewEntryList(SqlParameter[] paraObjects)
        {
            var dataSet = await _context.GetQueryDatatableAsync(StoredProcedureList.GetNewEntryList, paraObjects);
            return Common.ConvertDataTable<BUserEntryListDto>(dataSet.Tables[0]);
        }

        public async Task<List<BUserEntryListDto>> GetBusinessPartnerEntryList(SqlParameter[] paraObjects)
        {
            var dataSet = await _context.GetQueryDatatableAsync(StoredProcedureList.GetBusinessPartnerEntryList, paraObjects);
            return Common.ConvertDataTable<BUserEntryListDto>(dataSet.Tables[0]);
        }
        public async Task<List<BUserEntryListDto>> GetLogEntryList(SqlParameter[] paraObjects)
        {
            var dataSet = await _context.GetQueryDatatableAsync(StoredProcedureList.GetLogEntryList, paraObjects);
            return Common.ConvertDataTable<BUserEntryListDto>(dataSet.Tables[0]);
        }

        public async Task<List<BUserEntryListDto>> GetAdminDasboardEntryList(SqlParameter[] paraObjects)
        {
            var dataSet = await _context.GetQueryDatatableAsync(StoredProcedureList.GetAdminDashboardEntryList, paraObjects);
            return Common.ConvertDataTable<BUserEntryListDto>(dataSet.Tables[0]);
        }
        public async Task<List<BUserEntryListDto>> GetUserSide_B_HistoryList(SqlParameter[] paraObjects)
        {
            var dataSet = await _context.GetQueryDatatableAsync(StoredProcedureList.GetAdminDashboardEntryList, paraObjects);
            return Common.ConvertDataTable<BUserEntryListDto>(dataSet.Tables[0]);
        }
    }
}