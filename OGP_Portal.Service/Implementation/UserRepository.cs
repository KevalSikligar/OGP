using Microsoft.Data.SqlClient;
using OGP_Portal.Data.DbContext;
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
   public class UserRepository : GenericRepository<ApplicationUser>, IUserService
    {
        private readonly OGP_PortalContext _context;
        public UserRepository(OGP_PortalContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BUserDto>> GetBUserList(SqlParameter[] paraObjects)
        {
            var dataSet = await _context.GetQueryDatatableAsync(StoredProcedureList.GetB_UsersList, paraObjects);
            return Common.ConvertDataTable<BUserDto>(dataSet.Tables[0]);
        }

        public async Task<List<BPartnerDto>> GetBPartnerList(SqlParameter[] paraObjects)
        {
            var dataSet = await _context.GetQueryDatatableAsync(StoredProcedureList.GetB_PartnerList, paraObjects);
            return Common.ConvertDataTable<BPartnerDto>(dataSet.Tables[0]);
        }
    }
}
