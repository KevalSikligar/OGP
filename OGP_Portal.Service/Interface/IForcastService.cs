using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OGP_Portal.Data.DbModel;
using OGP_Portal.Service.Dtos;
using OGP_Portal.Service.Implementation.BaseService;

namespace OGP_Portal.Service.Interface
{
	public interface IForcastService : IGenericService<Forcast>
    {
        Task<List<ForcastDto>> GetForcastList(SqlParameter[] paraObjects);
    }
}
