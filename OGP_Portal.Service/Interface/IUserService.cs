using Microsoft.Data.SqlClient;
using OGP_Portal.Data.DbContext;
using OGP_Portal.Service.Dtos;
using OGP_Portal.Service.Implementation.BaseService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OGP_Portal.Service.Interface
{
    public interface IUserService : IGenericService<ApplicationUser>
    {
        Task<List<BUserDto>> GetBUserList(SqlParameter[] paraObjects);
        Task<List<BPartnerDto>> GetBPartnerList(SqlParameter[] paraObjects);
    }
}
