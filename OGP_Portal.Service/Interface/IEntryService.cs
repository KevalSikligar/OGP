using Microsoft.Data.SqlClient;
using OGP_Portal.Data.DbModel;
using OGP_Portal.Service.Dtos;
using OGP_Portal.Service.Implementation.BaseService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OGP_Portal.Service.Interface
{
   public  interface IEntryService : IGenericService<Entry>
    {
        Task<List<EntryDto>> GetEntryList(SqlParameter[] paraObjects);
        Task<List<OGPEntryDTO>> GetBalance(SqlParameter[] paraObjects);

        Task<List<BUserEntryListDto>> GetNewEntryList(SqlParameter[] paraObjects);
        Task<List<BUserEntryListDto>> GetBusinessPartnerEntryList(SqlParameter[] paraObjects);
        Task<List<BUserEntryListDto>> GetLogEntryList(SqlParameter[] paraObjects);
        Task<List<BUserEntryListDto>> GetAdminDasboardEntryList(SqlParameter[] paraObjects);
        Task<List<BUserEntryListDto>> GetUserSide_B_HistoryList(SqlParameter[] paraObjects);
    }
}
