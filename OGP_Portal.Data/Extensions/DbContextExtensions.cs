using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace OGP_Portal.Data.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task<DataSet> GetQueryDatatableAsync(this Microsoft.EntityFrameworkCore.DbContext context, string sqlQuery, SqlParameter[] sqlParam = null, CommandType type = CommandType.StoredProcedure)
        {
            using (DbCommand cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.Connection = context.Database.GetDbConnection();
                cmd.CommandType = type;
                if (sqlParam != null)
                { cmd.Parameters.AddRange(sqlParam); }
                cmd.CommandText = sqlQuery;
                using (DbDataAdapter dataAdapter = new SqlDataAdapter())
                {
                    dataAdapter.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    await Task.Run(() => dataAdapter.Fill(ds));
                    return ds;
                }
            }
        }

        public static DataSet GetQueryDatatable(this Microsoft.EntityFrameworkCore.DbContext context, string sqlQuery, SqlParameter[] sqlParam = null, CommandType type = CommandType.StoredProcedure)
        {
            using (DbCommand cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.Connection = context.Database.GetDbConnection();
                cmd.CommandType = type;
                if (sqlParam != null)
                { cmd.Parameters.AddRange(sqlParam); }
                cmd.CommandText = sqlQuery;
                using (DbDataAdapter dataAdapter = new SqlDataAdapter())
                {
                    dataAdapter.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    dataAdapter.Fill(ds);
                    return ds;
                }
            }
        }
    }
}

